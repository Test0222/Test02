using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class Rs232Param   //公共的232结构  头  格式   内容
    {
        public Int32 Index;
        public Byte enable;     // 表示使能位或者时间类别
        public Byte type;       // hex or ascii
        public String rsCmd;    // Rs232 commands 
        public Byte endFlag;    //结尾字符
    }
}
