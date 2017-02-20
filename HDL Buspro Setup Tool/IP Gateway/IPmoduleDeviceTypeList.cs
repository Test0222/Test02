using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class IPmoduleDeviceTypeList
    {
        public const Byte HowManyButtonsEachPage = 48; 

        public static int[] HDLIPModuleDeviceTypeLists = new int[] { 228, 229, 231, 232, 233, 234, 235, 236, 237, 238 ,740,741,742,743}; // 所有一端口交换机

        public static int[] IpModuleV1 = new int[] {233,234,235,236 };

        public static int[] IpModuleV2DHCP = new int[] {229,231,232 };

        public static int[] IpModuleV3DNS = new int[] { 228};

        public static int[] IpModuleV3TimeZoneUrl = new int[] { 228,3505,3506 };

        public static int[] RFIpModuleV1 = new int[] { 740, 741};

        public static int[] RFIpModuleV2 = new int[] { 742,743 };

        public static int[] IpModuleNewVersionHasBlockList = new int[] { };
    }
}
