using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HDL_Buspro_Setup_Tool
{
    public class ConvertorType
    {
        public Byte[] thirdPartAddress;
        public Int32 type;                  // hdl has a list , the number should be fixed
        public String deviceName;
        public Byte paramOne;
        public Byte paramTwo;
        public Byte paramThree;
        public Byte paramFour;
        public String remark;

        public ConvertorType()
        {
            thirdPartAddress = new Byte[4];
            type = 0;
            deviceName = "";
            paramOne = 0;
            paramTwo = 0;
            paramThree = 0;
            paramFour = 0;
            remark = "";
        }
    }
}
