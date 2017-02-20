using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HDL_Buspro_Setup_Tool
{
    public class DryContact
    {
        public Byte enable;                     // 当期按键有没有启用
        public Byte retriggerflag;              // 当期按键有没有重复触发使能
        public Byte mode;                       // 当期是哪个类型的开关   按键模式机械开关 或者电子开关
        public Byte type;                       //设备开关模块还是调光
        public Byte saveDimmer;                 //设备调光要不要保存
        public KeyState[] keyStates;            // = new KeyState[2];  //0 表示 ON  1 表示 OFF
    }

    public class KeyState
    {
        public Int32 state;
        public String remark;
        public Int32 actOnDelay;
        public Int32 actOffDelay;               // = 0;
        public List<UVCMD.ControlTargets> keyTargets;
    }

    public class KeyStateGenerationTwo : KeyState
    {
        public Byte[] avoidCut;                 // = new byte[10];//新版防拆功能
        public List<UVCMD.ControlTargets> cutAlarm;    // = null; //防拆报警目标
    }
}
