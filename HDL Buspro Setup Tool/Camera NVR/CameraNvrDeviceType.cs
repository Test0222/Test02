using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    class CameraNvrDeviceType
    {
        public const Byte cameraNvrBigType = 15;
        public const Byte cameraSmallType = 0;
        public const Byte nvrSmallType = 1;

        public static int[] cameraThirdPartDeviceType = new int[]{   // 所有摄像头 + NVR 备类型列表
            9000,9001,9002,9003,9020};

        public static int[] nvrThridPartDeviceType = new int[]{   //  NVR 备类型列表
            9020};
    }
}
