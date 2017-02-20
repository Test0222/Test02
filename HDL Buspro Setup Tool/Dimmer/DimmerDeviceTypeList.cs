using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class DimmerDeviceTypeList
    {
        public static int[] HDLDimmerDeviceTypeList = new int[]{ // 所有调光器设备类型列表
                1,2,3,4,5,6, 7,8, 9,13,14,15,17,36,37,40,41,42,43,44,45,46,
                21,25,26,27,28,30,31,35,41, 600,601,602,603,606,607,608,609,610,611,612,613,1015,852,614,615,616,617,
                618,619,620,621,455,454,854,622,623,624,625}; //混合模块界面特殊

        public static int[] DimmerModuleV1233 = new int[] { 1,2,3,4,5,6, 7,8, 9,13,14,15,17,36,37,40,42,43,44,45,46,
                21,25,26,27,28,30,31,35,41, 600,601,602,603,455,454,615,618,619,620,621}; //第二代调光器

        public static int[] DimmerModuleV2DimCurve = new int[] { 596,599,606,607,608,609,610,611,612,613,614,615,616,617, 618,619,620,621,622,1015,852
              }; //调光曲线和温度

        public static int[] DimmerModuleHasVoltageSelection = new int[] {44,45, 610, 611, 612, 614, 615, 616, 617,3501,3502 }; //110V 220V 电压选择

        public static int[] DimmerModuleHasTrailingLeading = new int[] { 606, 607, 608, 609, 610, 611, 612, 614, 615, 616, 617 }; // 前沿后沿选择

        public static int[] DimmerWithoutSequences = new int[] { 0x11, 36, 46, 43, 40, 41, 42,603}; // 没有序列

        public static int[] DALIDimmerDeviceTypeLists = new int[] { 40,41,42,43}; // DALI模块

        public static int[] DimmerMRDAHas0V1V = new int[] {454,455 };  //01-10v 或者前后沿
        public static int[] DimmerMRDAHasSeperateTraidingLeading = new int[] { 618,619,620,621 };  //01-10v 或者前后沿

        public static int[] HDLDMXDeviceType = new int[] // 所有DMX的设备类型
        {
           10,16,18, 20,21,32,851,24,39,850,854
        };

      
    }
}
