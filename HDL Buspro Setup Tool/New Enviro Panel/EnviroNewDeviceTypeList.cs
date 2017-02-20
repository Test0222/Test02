using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    class EnviroNewDeviceTypeList
    {
        public static int[] EnviroNewPanelDeviceTypeList = new int[] { 182,185 };//彩色DLP 调整协议后

        public static Byte SetupPage = 1;
        public static Byte Standby = 2;

        public static Byte ButtonStart  =3;
        public static Byte ButtonEnd = 38;

        public static Byte ACStart = 39;
        public static Byte ACEnd = 47;

        public static Byte FHStart = 48;
        public static Byte FHEnd = 56;

        public static Byte MusicStart = 57;
        public static Byte MusicEnd = 65;
 
    }
}
