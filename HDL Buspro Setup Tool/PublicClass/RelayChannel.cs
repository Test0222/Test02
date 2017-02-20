using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HDL_Buspro_Setup_Tool
{
    public class RelayChannel
    {
        public int ID;
        public string Remark;
        public int LoadType;
        public int OnDelay;
        public int ProtectDelay;
        public int OFFDelay;
        public int OFFProtectDelay;
        public int intBelongs;
        public byte bytOnStair; // 0 no install on stair   1 means on stair
        public int intONTime; // will go off automatically 0 always on

        //RelayChannelGenerationThree  2016 12 26 new add
        public Byte bEnableChn;        // 1 means enable; 0 disable
        public Byte bEnableCurtain;    // enable curtain function
        public Int32 onTime;        // will go off automatically, 0 always on
    }

    public class RelayChannelGenerationTwo : RelayChannel
    {
        public Byte onStair;        // 0 no install on stair, 1 means on stair
        public Int32 onTime;        // will go off automatically, 0 always on
    }
}
