using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class Camera
    {
        public int dIndex;//设备唯一编号
        public bool[] read2UpFlags = new bool[4]; //

        //基本信息
        public string Devname;//子网ID 设备ID 备注
        public string UserName; // 登录用户名
        public string Password; // 密码

        public NetworkInformation networkInformation;
    }

    public class Nvr : Camera
    {
        public List<string[]> cameraLists;
    }
}
