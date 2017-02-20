using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class ConstMMC
    {
        public const byte MaxUVSwitchCount = 200;
        public const byte EveryUVMaxRS232CMDCount = 8;
        public const byte MaxIRDeviceCount = 24;
        public const string IRsperatedLabel = "/";

       public static int[] MediaPBoxDeviceType = new int[]{  //所有多媒体播放设备
            908};
    }
}
