using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.OleDb;

namespace HDL_Buspro_Setup_Tool
{
    /// <summary>
    /// 公共Load按键模式
    /// </summary>
    public class ButtonMode
    {
        public Byte ModeOrder; // 排序ID
        public Byte ModeSaveID;  // 保存用的ID
        public String ModeName; //中文

        public static void LoadButtonModeFromDatabaseToPublicClass()
        {
            CsConst.myPublicButtonMode = new List<ButtonMode>();

            string strsql = string.Format("select * from defKeyMode order by KeyModeNO");
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrDefaultPath);
            
            if (dr != null)
            {
                while (dr.Read())
                {
                    ButtonMode TmpButtonMode = new ButtonMode();
                    TmpButtonMode.ModeOrder = (Byte)(dr.GetInt16(0));
                    TmpButtonMode.ModeSaveID = (Byte)(dr.GetInt16(1));
                    if (CsConst.iLanguageId == 0) //
                    {
                        TmpButtonMode.ModeName = dr.GetString(3);
                    }
                    else
                    {
                        TmpButtonMode.ModeName = dr.GetString(2);
                    }
                    CsConst.myPublicButtonMode.Add(TmpButtonMode);
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
        public static byte ConvertorKeyModesToPublicModeGroup(String TmpButtonMode)
        {
            byte SaveID = 0;
            if (CsConst.myPublicButtonMode == null) return SaveID;

            foreach (ButtonMode oTmp in CsConst.myPublicButtonMode)
            {
                if (oTmp.ModeName == TmpButtonMode)
                {
                    SaveID = oTmp.ModeSaveID;
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
            if (CsConst.myPublicButtonMode == null) return "";
            string ButtonMode = CsConst.myPublicButtonMode[0].ModeName;
            
            foreach (ButtonMode oTmp in CsConst.myPublicButtonMode)
            {
                if (oTmp.ModeSaveID == TmpButtonMode)
                {
                    ButtonMode = oTmp.ModeName;
                    break;
                }
            }
            return ButtonMode;
        }

        //根据不同的类型显示按键模式
        public static string[] DisplayKeyModesAccordinglyDeviceType(int DeviceType)
        {
            //动态载入所有 以备上传使用 建议修改 读取时 调用 以防万一
            String[] strResult = null;
            List<string> TmpButtonModes = new List<string>();  //返回结果
            // 只要基本的类型
            int[] intBasicModeDeviceType = new int[] { 59, 2009, 742 ,308,309,321,322,181}; // 单一 + 组合+点动          

            if (CsConst.myPublicButtonMode == null) return strResult;

            if (intBasicModeDeviceType.Contains(DeviceType) || IPmoduleDeviceTypeList.RFIpModuleV2.Contains(DeviceType))  // 基本按键模式
            { 
                foreach (ButtonMode oTmp in CsConst.myPublicButtonMode)
                {
                    Byte KeyFunType = oTmp.ModeSaveID;
                    if (KeyFunType <= 7 || KeyFunType == 11)
                    {
                        TmpButtonModes.Add(oTmp.ModeName);
                    }
                }
            }
            else if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(DeviceType)) // DLP
            {
                foreach (ButtonMode oTmp in CsConst.myPublicButtonMode)
                {
                    TmpButtonModes.Add(oTmp.ModeName);
                }
            }
            else if (NormalPanelDeviceTypeList.HDLNormalPanelDeviceTypeList.Contains(DeviceType))
            {
                foreach (ButtonMode oTmp in CsConst.myPublicButtonMode)
                {
                    Byte[] KeyMode = new Byte[] { 9, 10, 11, 15, 16,17 };
                    Byte KeyFunType = oTmp.ModeSaveID;
                    if (KeyFunType <= 7 || KeyMode.Contains(KeyFunType))
                    {
                        TmpButtonModes.Add(oTmp.ModeName);
                    }
                }
            }
            else if (MHICDeviceTypeList.HDLCardReaderDeviceType.Contains(DeviceType)) // 酒店插卡取电模块
            {
                foreach (ButtonMode oTmp in CsConst.myPublicButtonMode)
                {
                    Byte[] KeyMode = new Byte[] { 11, 10, 14};
                    Byte KeyFunType = oTmp.ModeSaveID;
                    if (KeyFunType < 6 || KeyMode.Contains(KeyFunType))
                    {
                        TmpButtonModes.Add(oTmp.ModeName);
                    }
                }
            }

            return TmpButtonModes.ToArray();

        }
    }
}
