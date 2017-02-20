using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class RFSetup
    {
        public Int32 ssid;
        public Byte gatewaySubnetId;
        public Byte gatewayDeviceId;
        public Byte frequencyBand;
        public Byte channel;
        public Byte[] password;

        public RFSetup()
        {
            ssid = 0;
            gatewaySubnetId = 0;
            gatewayDeviceId = 0;
            frequencyBand = 0;
            channel = 0;
            //password = new Byte[];
        }
    }
}
