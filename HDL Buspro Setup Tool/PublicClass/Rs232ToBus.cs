using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class Rs232ToBus
    {
        public Int32 ID;
        public String remark;
        public Rs232Param rs232Param;
        public List<UVCMD.ControlTargets> busTargets;
    }
}
