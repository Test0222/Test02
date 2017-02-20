using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class SequenceStep
    {
        public Byte sceneID;
        public Byte runTimeHigh;
        public Byte runTimeLow;

        public SequenceStep()
        {
            sceneID = 0;
            runTimeHigh = 0;
            runTimeLow = 0;
        }
    }

    public class Sequence
    {
        public String remark;
        public Byte mode;
        public Byte runTimes;               //运行次数
        public Byte steps;                  //总共多少步
        public List<SequenceStep> runStep;  //运行步骤

        public Sequence()
        {
            remark = "";
            mode = 0;
            runTimes = 0;
            steps = 0;
            runStep = new List<SequenceStep>();
        }
    }

    public class RelaySequence : Sequence
    {
        public Byte lastStep;               // 1表示停留在最后一步, 0表示回复序列前状态, 默认为0

        public RelaySequence()
            : base()
        {
            lastStep = 0;
        }
    }
}
