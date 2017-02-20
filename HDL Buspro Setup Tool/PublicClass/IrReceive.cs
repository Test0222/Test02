using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HDL_Buspro_Setup_Tool
{
    public class IrReceive
    {
        public Int32 buttonId;
        public String buttonRemark;
        public Byte buttonMode;
        public List<UVCMD.ControlTargets> targetsInfo;
    }
}
