using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class HVACModuleDeviceTypeList
    {
        public static int[] HDLHVACModuleDeviceTypeLists = new int[] { // 所有HVAC的设备类型列表
            106,107,111,112,726,730,731,732,733,734,735,736,737,738,739};

        public static int[] HVACG1NormalRelayDeviceTypeLists = new int[] { // 带继电器序列的第一代HVAC
            106,107,111,112,730,731};

        public static int[] HVACG2ComplexRelayDeviceTypeLists = new int[] { // 复杂模式HVAC配置
            726,732,733,734,735,736,737,738,739};

        public static int[] HVACG2ComplexRelayHostDeviceTypeLists = new int[] { // 复杂模式HVAC配置
            726,737,738,739}; 


    }
}
