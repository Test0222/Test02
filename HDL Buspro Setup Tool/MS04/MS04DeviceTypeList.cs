using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class MS04DeviceTypeList
    {
        public static int[] HDLMS04DeviceTypeList = new int[] {//所有ms04设备类型
           113,114,115,116,119,118,121,93,141,140,142,135,136,137,351,352,353,354,355,356,357,358,359,360,361,362,363,364,365,366,367,
           1052,1053,1054,1055,1056,
           5900,
           5302,5303,
           5500, 5501,5502,5503,
           5700, 5701, 5702};

        public static int[] MS04G1WithoutSecurityDeviceTypeList = new int[] {//所有ms04没有安防功能页面
           113,114,115,116,119,118,121,93,141,140,142,135,136,352,353,363,364,365,
           5302,5303,
           5500, 5501,5502,5503,
           5700, 5701, 5702,
           363};

        public static int[] MS04NormalDeviceTypeList = new int[] {//所有ms04设备类型
           113,114,115,116,119,118,121,93,141,140,142,135,136,137,351,352,353,354,355,356};

        public static int[] MS04GetTameredDeviceTypeList = new int[] {//MS04防拆功能
           355,357,361,366,367};

        public static int[] MS04IOModuleDeviceTypeList = new int[] { 363};  // 六路干节点

        public static int[] MS04WirelessDeviceTypeList = new int[] { 5900};


        public static int[] MS04HotelMixModule = new int[] { 1052, 1053, 1054, 1055, 1056 };  //印尼混合模块

        public static int[] MS04HotelMixModuleHasArea = new int[] { 1056 };   //印尼混合模块 有区域设置


        public static int[] WirelessMS04WithOneCurtain = new int[] { 5302,5303 };//一路窗帘驱动器

        public static int[] WirelessMS04WithRelayChn = new int[] { 5500, 5501, 5502,5503 };//继电器驱动器

        public static int[] WirelessMS04WithDimmerChn = new int[] {5700, 5701, 5702 };//调光驱动器

        public static int[] MS04IModuleWithTemperature = new int[] { 365}; // 干接点可接温度探头

        public static int[] MS04IModuleWithCombinationONAndOFF = new int[] { 366,367,1056 }; // 干接点组合开关分别存储目标

        public static int[] MS04DryContactWithE000PublicCMD = new int[] { 5900,5302,5303,    // 目标以新的操作码来读写
           366,367,1056,
           5500, 5501,5502,5503,
           5700, 5701, 5702};
    }
}
