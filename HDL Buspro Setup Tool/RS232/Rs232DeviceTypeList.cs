using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class Rs232DeviceTypeList
    {
        public static int[] HDLRs232DeviceType = new int[]{//所有rs232模块设备类型列表
            1004, //地热转换器
            1007,1008,1009,1013,1014,1016,1017,1018,1019, 1020,1024,1027,1028,1029,1030,1031,1033,1034,1037,1039,1040,1041,1042};

        public static int[] Rs233CurtainConvertorGateway = new int[]{ // 窗帘转换器
             1037,1042};

        public static int[] Rs233AcConvertorGateway = new int[]{ // 空调转换器
             1064,1040,1041};

        public static int[] Rs233FhConvertorGateway = new int[]{ // 地热转换器
             1004};


       public static int[] Rs232ommaxConvertorGateway = new int[]{ // Commax
                    1039};

       public static int[] Rs232FilterConvertorGateway = new int[]{ // 有操作码过滤功能列表
                    1033};

       public static int[] Rs232DoesNotHave232485Tab = new int[] { // 不含232 485 配置页面
            1039,1037,1033,1028,1004,1042};
    }
}
