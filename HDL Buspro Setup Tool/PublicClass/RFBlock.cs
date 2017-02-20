using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class RFBlock
    {
        public Byte isEnable;           // 是否启用
        public Byte blockType;          // 拦截类型
        public Byte sourceSubnetID;     // send id 
        public Byte sourceDeviceID;     // device id 
        public Int32 command;           //操作码
        public Byte destSubnetID;       // sub id 
        public Byte destDeviceID;       // device id 

        public RFBlock()
        {
            isEnable = 0;
            blockType = 0;
            sourceSubnetID = 0;
            sourceDeviceID = 0;
            command = 0;
            destSubnetID = 0;
            destDeviceID = 0;
        }

        public Boolean ModifyfBlockSetupInformation(Byte SubNetID, Byte DeviceID, Byte BlockID)  
        {
            Boolean BlnIsSuccess = false;
            byte[] arayTmp = new byte[9];
            arayTmp[0] = BlockID;
            arayTmp[1] = isEnable;
            arayTmp[2] = blockType;
            arayTmp[3] = sourceSubnetID;
            arayTmp[4] = sourceDeviceID;
            arayTmp[5] = (byte)(command / 256);
            arayTmp[6] = (byte)(command % 256);
            arayTmp[7] = sourceSubnetID;
            arayTmp[8] = sourceDeviceID;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1D44, SubNetID, DeviceID, false, true, true, false) == false)
            {
                return BlnIsSuccess;
            }
            BlnIsSuccess = true;
            return BlnIsSuccess;
        }

        public Boolean ReadRfBlockSetupInformation(Byte SubNetID, Byte DeviceID, Byte BlockID)
        {
            Boolean BlnIsSuccess = false;
            try
            {
                Byte[] ArayTmp = new byte[1] { BlockID };
                CsConst.MybytNeedParm1 = BlockID;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D42, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    isEnable = CsConst.myRevBuf[26]; // net work information
                    blockType = CsConst.myRevBuf[27];
                    sourceSubnetID = CsConst.myRevBuf[28]; // 子网掩码
                    sourceDeviceID = CsConst.myRevBuf[29];
                    command = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31]; //0 局域网，1 p2p, 2 server
                    destSubnetID = CsConst.myRevBuf[32];
                    destDeviceID = CsConst.myRevBuf[33];
                    BlnIsSuccess = true;
                }
            }
            catch { }
            return BlnIsSuccess;
        }
    }
}
