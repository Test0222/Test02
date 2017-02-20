using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class AudioDeviceTypeList
    {
        public static bool isEndReceiveFile = false;

        public static int[] AudioBoxDeviceTypeList = new int[]{  // 所有的背景音乐设备类型
            901,902,903,904,905,906,907, 910, 911,912, 913
        };

        public static int[] MzBoxDeviceType = new int[] { 906, 910, 911,912, 913 };//所有音乐盒子设备类型

        public static int[] DinRailAudioDeviceType = new int[] { 911}; // 丁导轨系列 没有蓝牙等

        public static int[] AudioBoxHasPartyGroup = new int[] { 906, 910};
    }
}
