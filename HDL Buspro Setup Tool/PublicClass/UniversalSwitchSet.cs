using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class UniversalSwitchSet
    {
        public Byte id;
        public String remark;
        public Byte condition;          // on or off
        public Boolean isAutoOff;
        public Int32 autoOffDelay;
        public Byte state;

        public UniversalSwitchSet()
        {
            id = 0;
            remark = "";
            condition = 0;
            isAutoOff = false;
            autoOffDelay = 0;
        }
    }
}
