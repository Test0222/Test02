using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HDL_Buspro_Setup_Tool
{
    public class IrSequence
    {
        public String remark;
        public Byte enable;
        public List<Step> steps;

        public class Step
        {
            public Byte type;
            public Byte paramOne;
            public Byte paramTwo;
            public Byte paramThree;
            public Byte paramFour;
            public Int32 delay;
        }
    }
}
