using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class Area
    {
        public String remark;
        public List<Scene> scene;

        public Area()
        {
            remark = "";
            scene = new List<Scene>();
        }
    }

    public class AreaGenerationOne : Area
    {
        public List<Sequence> sequence;
        public Byte defaultScene;       // = 0;  // 上电恢复, FF表示掉电前场景

        public AreaGenerationOne()
        {
            sequence = new List<Sequence>();
            defaultScene = 0;
        }
    }
}
