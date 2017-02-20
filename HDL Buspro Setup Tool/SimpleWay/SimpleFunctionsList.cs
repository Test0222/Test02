using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDL_Buspro_Setup_Tool
{
    public class SimpleFunctionsList
    {
        public List<SimpleFunction> currentSearchResult;

        public const Byte lightBigType = 1;
        public const Byte curtainBigType = 2;
        public const Byte buttonBigType = 4;
        public const Byte sensorBigType = 5;

        public static List<HdlSimpleLights> simpleLightsList = null;
        public static List<HdlSimpleCurtains> simpleCurtainsList = null;
        public static List<HdlSimpleButtons> simpleButtonsList = null;
        public static List<HdlSimpleSensors> simpleSensorsList = null;
    }


}
