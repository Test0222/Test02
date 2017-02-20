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
    public class DeviceTypeList
    {
        public int DeviceType;  // 保存用的ID
        public String sModelChn; // 中文型号
        public String sModelEng; // 英文型号
        public String DeviceName; //中文
        public int iMaxValue;
        public String sSimpleFunctionList; // 简易功能分解列表

        public static void LoadButtonCoontrolTypeFromDatabaseToPublicClass()
        {
            CsConst.myDeviceTypeLists = new List<DeviceTypeList>();
            try
            {
                string strsql = string.Format("select * from defDeviceType order by DeviceType");
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrDefaultPath);
                
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        DeviceTypeList TmpButtonMode = new DeviceTypeList();
                        TmpButtonMode.DeviceType = dr.GetInt32(0);
                        if (dr.GetString(1) != null)
                            TmpButtonMode.sModelChn = dr.GetString(1);
                        if (dr.GetString(2) != null)
                            TmpButtonMode.sModelEng = dr.GetString(2);
                        if (CsConst.iLanguageId == 1)
                        {
                            TmpButtonMode.DeviceName = dr.GetString(3);
                            if (TmpButtonMode.DeviceName.Contains('('))
                            {
                                TmpButtonMode.DeviceName = TmpButtonMode.DeviceName.Split('(')[0].ToString();
                            }
                        }
                        else
                        {
                            TmpButtonMode.DeviceName = dr.GetString(4);
                        }
                        TmpButtonMode.iMaxValue = dr.GetInt32(5);
                        if (TmpButtonMode.DeviceType >=60000)
                        {
                            dr.Close();
                            return;
                        }
                        else
                        {
                            if (dr.GetString(13) != null)
                                TmpButtonMode.sSimpleFunctionList = (String)dr.GetString(13);
                        }
                        CsConst.myDeviceTypeLists.Add(TmpButtonMode);
                    }
                    dr.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 获取设备类型号 描述
        /// </summary>
        /// <param name="bytKeyMode"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>      
        public static String[] GetDisplayInformationFromPublicModeGroup(int DeviceType)
        {
            String[] sDisplayList = new String[2];
            if (CsConst.myPublicControlType == null) return sDisplayList;
            try
            {
                foreach (DeviceTypeList oTmp in CsConst.myDeviceTypeLists)
                {
                    if (oTmp.DeviceType == DeviceType)
                    {
                        sDisplayList = new String[] { oTmp.sModelEng, oTmp.DeviceName };
                        break;
                    }
                }
            }
            catch 
            {
                return sDisplayList;
            }
            return sDisplayList;
        }

        /// <summary>
        /// 获取简易编程的功能列表
        /// </summary>
        /// <param name="bytKeyMode"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>      
        public static String GetSimpleFunctionListFromPublicModeGroup(int DeviceType)
        {
            if (CsConst.myDeviceTypeLists == null) return "";

            string ButtonMode = "";

            foreach (DeviceTypeList oTmp in CsConst.myDeviceTypeLists)
            {
                if (oTmp.DeviceType == DeviceType)
                {
                    ButtonMode = oTmp.sSimpleFunctionList;
                    break;
                }
            }
            return ButtonMode;
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="bytKeyMode"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>      
        public static Int32 GetMaxValueFromPublicModeGroup(int DeviceType)
        {
            if (CsConst.myDeviceTypeLists == null) return 0;

            int ButtonMode = 0;

            foreach (DeviceTypeList oTmp in CsConst.myDeviceTypeLists)
            {
                if (oTmp.DeviceType == DeviceType)
                {
                    ButtonMode = oTmp.iMaxValue;
                    break;
                }
            }
            return ButtonMode;
        }
    }
}
