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
    public class DryMode
    {
        public Byte ModeSaveID;  // 保存用的ID
        public String ModeName; //中文

        public static void LoadButtonModeFromDatabaseToPublicClass()
        {
            CsConst.myPublicDryMode = new List<DryMode>();

            string strsql = string.Format("select * from DryMode order by DryMode");
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrDefaultPath);
            
            if (dr != null)
            {
                while (dr.Read())
                {
                    DryMode TmpButtonMode = new DryMode();
                    TmpButtonMode.ModeSaveID = (Byte)(dr.GetInt16(0));
                    if (CsConst.iLanguageId == 0) //
                    {
                        TmpButtonMode.ModeName = dr.GetString(2);
                    }
                    else
                    {
                        TmpButtonMode.ModeName = dr.GetString(1);
                    }
                   
                    CsConst.myPublicDryMode.Add(TmpButtonMode);
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
            if (CsConst.myPublicDryMode == null) return SaveID;

            foreach (DryMode oTmp in CsConst.myPublicDryMode)
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
            if (CsConst.myPublicDryMode == null) return "";
            string ButtonMode = CsConst.myPublicDryMode[0].ModeName;

            foreach (DryMode oTmp in CsConst.myPublicDryMode)
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
            int[] intDry4zModes = new int[] { 119, 118, 115, 114, 113, 93, 141, 140, 354, 137,3504 };  //新的MS04模块

            if (CsConst.myPublicDryMode == null) return strResult;

            if (MS04DeviceTypeList.MS04IModuleWithCombinationONAndOFF.Contains(DeviceType)) // 组合开关分别存储目标
            {
                foreach (DryMode oTmp in CsConst.myPublicDryMode)
                {
                    Byte KeyFunType = oTmp.ModeSaveID;
                    if (KeyFunType != 100)
                    {
                        TmpButtonModes.Add(oTmp.ModeName);
                    }
                }
            }
            else if (MS04DeviceTypeList.MS04IModuleWithTemperature.Contains(DeviceType)) // 带温度感应探头
            {
                foreach (DryMode oTmp in CsConst.myPublicDryMode)
                {
                    Byte KeyFunType = oTmp.ModeSaveID;
                    if (KeyFunType !=9)
                    TmpButtonModes.Add(oTmp.ModeName);
                }
            }
            else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(DeviceType) 
                  || HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(DeviceType)
                  || MHICDeviceTypeList.HDLCardReaderDeviceType.Contains(DeviceType))  // 基本按键模式
            {
                foreach (DryMode oTmp in CsConst.myPublicDryMode)
                {
                    Byte KeyFunType = oTmp.ModeSaveID;
                    if (KeyFunType <= 8)
                    {
                        TmpButtonModes.Add(oTmp.ModeName);
                    }
                }
            }
            return TmpButtonModes.ToArray();
        }
    }
}
