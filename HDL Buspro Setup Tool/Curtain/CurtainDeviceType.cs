using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    class CurtainDeviceType
    {
        public static int[] HDLCurtainModuleDeviceType = new int[] {//所有Curtain设备类型
            700,701,702,703,704,705,706,707,713,714,708, 710, 712, 5300, 709, 711, 5301,717,715,
            718,719};   //20170205改动，增加719

        public static int[] NormalCurtainG1DeviceType = new int[] {//第一代Curtain设备类型
            700,701,702,703,704,705,706,707};

        public static int[] CurtainG2DeviceType = new int[] { 713,714,719}; //第二代窗帘模块   //20170205改动增加719

        public static int[] NormalMotorCurtainDeviceType = new int[] { 708, 710, 712,715, 5300 };//所有开合帘

        public static int[] RollerCurtainDeviceType = new int[] { 709, 711, 5301,717,718 };//所有卷帘控制器

        public static int[] WirelessCurtainDriverDeviceType = new int[] { 5302 };//一路窗帘驱动器
    }
}
