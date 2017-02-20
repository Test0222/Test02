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
    public class ButtonControlType
    {
        public Byte ControlTypeSaveID;  // 保存用的ID
        public String ControlTypeName; //中文

        public static void LoadButtonCoontrolTypeFromDatabaseToPublicClass()
        {
            CsConst.myPublicControlType = new List<ButtonControlType>();

            string strsql = string.Format("select * from defKeyFunType order by KeyFunType");
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrDefaultPath);
            
            if (dr != null)
            {
                while (dr.Read())
                {
                    ButtonControlType TmpButtonMode = new ButtonControlType();
                    TmpButtonMode.ControlTypeSaveID =(Byte) dr.GetInt16(0);
                    if (CsConst.iLanguageId == 0) //
                    {
                        TmpButtonMode.ControlTypeName = dr.GetString(2);
                    }
                    else
                    {
                        TmpButtonMode.ControlTypeName = dr.GetString(1);
                    }
                    CsConst.myPublicControlType.Add(TmpButtonMode);
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
            if (CsConst.myPublicControlType == null) return SaveID;

            foreach (ButtonControlType oTmp in CsConst.myPublicControlType)
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
            if (CsConst.myPublicControlType == null) return "";
            string ButtonMode = CsConst.myPublicControlType[0].ControlTypeName;

            foreach (ButtonControlType oTmp in CsConst.myPublicControlType)
            {
                if (oTmp.ControlTypeSaveID == TmpButtonMode)
                {
                    ButtonMode = oTmp.ControlTypeName;
                    break;
                }
            }
            return ButtonMode;
        }

        //根据不同的类型显示按键模式
        public static void AddControlTypeToControl(ComboBox cboType, int deviceType)
        {
            if (CsConst.myPublicControlType == null || CsConst.myPublicControlType.Count == 0) return;
            try
            {
                cboType.Items.Clear();
                if (IPmoduleDeviceTypeList.RFIpModuleV2.Contains(deviceType)) // RF 网线网关
                {
                    foreach (ButtonControlType oTmp in CsConst.myPublicControlType)
                    {
                        Byte KeyFunType = oTmp.ControlTypeSaveID;
                        if (KeyFunType <= 103 && KeyFunType != 87 && KeyFunType != 93 && KeyFunType != 96)
                        {
                            cboType.Items.Add(oTmp.ControlTypeName);
                        }
                    }
                }
                //string sql = "select * from defKeyFunType ";
                //if (CsConst.minSameControlOfDryCont.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.minMs04DeviceType.Contains(deviceType) && deviceType != 5900)
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.mintColorDLPDeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.mintGPRSDeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 94 and KeyFunType <> 96";
                //}
                //else if (CsConst.mintMPBDeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.minAllPanelDeviceType.Contains(deviceType))
                //{
                //    if (CsConst.mintDLPDeviceType.Contains(deviceType))
                //    {
                //        if (CsConst.mintNewDLPFHSetupDeviceType.Contains(deviceType))
                //            sql = sql + "where KeyFunType <> 96 and KeyFunType <> 106 and KeyFunType <> 107 ";
                //        else
                //            sql = sql + "where KeyFunType <= 108 and KeyFunType <> 96 and KeyFunType <> 106 and KeyFunType <> 105 and KeyFunType <> 107";

                //    }
                //    else
                //    {
                //        sql = sql + "where KeyFunType <= 108 and KeyFunType <> 96 and KeyFunType <> 106 and KeyFunType <> 105 and KeyFunType <> 107";
                //    }
                //}
                //else if (CsConst.minDoorBellDeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.minCardReaderDeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.minMSPUBRFDeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.minRs232DeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.min12in1DeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.mintSensor_MINIULDeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.minSensor_7in1DeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //else if (CsConst.minMSPUDeviceType.Contains(deviceType))
                //{
                //    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                //}
                //dr = DataModule.SearchAResultSQLDB(sql);
                //if (dr != null)
                //{
                //    while (dr.Read())
                //    {
                //        if (CsConst.iLanguageId == 1) str = dr.GetValue(1).ToString();
                //        else if (CsConst.iLanguageId == 0) str = dr.GetValue(2).ToString();
                //        cb.Items.Add(str);
                //    }
                //    dr.Close();
                //}
            }
            catch
            {
                //dr.Close();
            }
        }
    }
}
