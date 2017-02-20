using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class Scene
    {
        public String remark;
        public Int32 time;
        public List<Byte> light;

        public Scene()
        {
            remark = "";
            time = 0;
            light = new List<Byte>();
        }
    }
}