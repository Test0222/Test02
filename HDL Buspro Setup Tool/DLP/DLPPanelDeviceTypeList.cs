using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class DLPPanelDeviceTypeList
    {
        public static int[] HDLDLPPanelDeviceTypeList = new int[]{  // 所有液晶面板设备类型列表
            48,86,87,149,154,155,156,157,158,160,162,165,167,168,170,172,175,176,180,5000,5001,5002,5004            
        };

        public static int[] WirelessDLPDeviceTypeList = new int[] { // 所有无线液晶面板设备类型列表
            5000,5001,5002,5004,5003
        };

        public static int[] ColorfulDLPDeviceType = new int[] { 165, 168, 170, 172, 175};//十四按键DLP


        public static int[] DLPWithNewMusicSources = new int[] {157,159,162,165,158,160,156,180,167,168,170,171,175,172,173,5000,5001,5002,5003,5004};
 
    }
}
