using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class DMXDeviceTypeList
    {
        public static int[] HDLDMXDeviceTypeList = new int[]{
            1600,5800
        };  // 所有DMX模块

        public static int[] DMXHasPowerSavingFunction = new int[]{5800  //所有带节能的DMX模块
        };
    }
}
