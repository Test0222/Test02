using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class BusToRs232
    {
        public String remark;
        public Byte type;
        public Byte paramOne;
        public Byte paramTwo;
        public List<Rs232Param> rs232Cmd;
    }  
}
