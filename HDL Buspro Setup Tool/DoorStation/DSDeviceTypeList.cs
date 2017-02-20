using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class DSDeviceTypeList
    {
        public static int[] HDLDSDeviceTypeList = new int[]{
            3200,3201
        };  // 所有DMX模块

        public static int[] DoorStationDeviceType = new int[] { 3200 };///十寸屏室外机
        public static int[] NewDoorStationDeviceType = new int[] { 3201 };///新门口机 
    }
}
