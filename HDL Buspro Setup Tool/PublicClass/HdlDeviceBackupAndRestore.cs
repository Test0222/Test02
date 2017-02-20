using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class HdlDeviceBackupAndRestore :ICloneable
    {
        public Byte subnetIDBackup;
        public Byte deviceIDBackup;
        public String remarkBackup;
        public Int32 DeviceTypeBackup;
        public Int32 iIndexBackup;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void SetToDefault()
        {
            subnetIDBackup = 0;
            deviceIDBackup = 0;
            remarkBackup = "";
            DeviceTypeBackup = 0;           
        }
    }
}
