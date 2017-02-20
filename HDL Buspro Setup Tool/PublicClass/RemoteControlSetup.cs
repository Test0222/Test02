using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class RemoteControlSetup
    {
        public Byte workMode;           // local netowrk or ethernet mode or remote control
        public String group;
        public String projectName;
        public String user;
        public String password;
        public Byte[] serverOne;
        public Int32 serverPortOne;
        public Byte[] serverTwo;
        public Int32 serverPortTwo;
        public Byte timer;
        public Byte enableDHCP;

        public RemoteControlSetup()
        {
            workMode = 0;
            group = "";
            projectName = "";
            user = "";
            password = "";

            serverOne = new Byte[4];
            serverPortOne = 0;

            serverTwo = new Byte[4];
            serverPortTwo = 0;

            timer = 0;
            enableDHCP = 0;
        }
    }
}
