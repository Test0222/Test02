using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class DimmerChannel 
    {
        public int id;
        public String remark;
        public Int32 loadType;
        public Int32 minValue;
        public Int32 maxValue;
        public Int32 maxLevel;
        public Int32 curveId;
        public Int32 belongArea;
        public Byte[] curStatus; //当前状态

        public DimmerChannel()
        {
            remark = "";
            loadType = 0;
            minValue = 0;
            maxValue = 0;
            maxLevel = 0;
            belongArea = 0;
        }
    }

    public class DimmerChannelGenerationTwo : DimmerChannel
    {
        public Byte dimmingProfile;

        public DimmerChannelGenerationTwo()
            : base()
        {
            dimmingProfile = 0;
        }
    }
}
