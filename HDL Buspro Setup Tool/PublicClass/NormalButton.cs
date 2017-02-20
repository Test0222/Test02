using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace HDL_Buspro_Setup_Tool
{
    public class NormalButton
    {
        public String remark;
        public Byte mode;
        public Byte isLedOn;
        public Byte isDimmer;
        public Byte saveDimmer;
        public Byte mutex;                                      //是不是互斥
    }

    public class TouchButton : NormalButton
    {
        public Byte[] onColor;
        public Byte[] offColor;
    }

    public class DLPButton : NormalButton
    {
        public Image onIcon;
        public Image offIcon;
    }
}
