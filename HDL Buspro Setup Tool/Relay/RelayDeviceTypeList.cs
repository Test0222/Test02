using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class RelayDeviceTypeList
    {
        public static int[] HDLRelayDeviceTypeList = new int[]{  // 所有继电器设备类型列表
           423,424,425,426,427,428,429,430,431,432,433,434,435,436,437,438,439,440,441,442,443,444,445,446,447,448,449,450,451,
           457,
           11,12,19,22,23,
           150,151,152,153,
           458,459,460,461,462,463,464,465,466};

        public static int[] RelayModuleV1 = new int[] {233,234,235,236 };

        public static int[] RelayHasBroadcastFlag = new int[] { 423, 441, 442, 443, 444, 445, 446, 447, 448, 449, 450, 451 };//楼梯灯

        public static int[] RelayModuleV2StairGroup = new int[] { 423, 441, 442, 443, 444, 445, 446, 447, 448, 449, 450, 451,457 };//楼梯灯

        public static int[] RelayModuleWorkAsCurtainControlGroup = new int[] { 458, 459, 460, 461, 462, 463, 464, 465, 466 }; // 继电器可以当 窗帘模块使用

        public static int[] RelayModuleWithButtonEnableGroup = new int[] {462, 463, 464, 466 }; // 继电器可以使能按键

        public static int[] RelayModuleV2OffDelay = new int[] { 444, 445, 446, 447, 448, 449, 450, 451,454,457};

        public static int[] RelayWithoutSequences = new int[] { 458, 459, 460, 462,464,457 }; // 没有序列

        public static int[] RelayWithOutLoadTypeReading = new int[] { 454,457}; // 没有负载类型的设置

        public static int[] RelayThreeChnBaseWithACFunction = new int[] {457 };
    }
}
