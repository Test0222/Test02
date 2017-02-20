using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace HDL_Buspro_Setup_Tool
{
    class CsConst
    {
        public static int mintDIndex = 1;  // used for add devcie dindex

        public static IniFile mstrINIDefault;///对LAN0,LAN1进行操作

        public static List<int> AddedDevicesIndexGroup = null;

        /// <summary>
        /// Main Form Const Start
        /// </summary>
        #region
        public static int MainFormSubNetIDStartFrom = 3;
        #endregion

        // public static Inifile

        // all different kinds of modules and their device type
        #region

        public static int[] constCRCTab1 = new int[] {
        	  0xe1ce,0x1021,0x3063,0xc18c,0x50a5,0xa14a,0x70e7,0x8108,
              0x60c6,0x9129,0x4084,0xb16b,0x2042,0xd1ad,0xf1ef,0x0000}; // new CRC table

        public static int[] SpecailAddress = new int[] { 
              36,37,41,42,43,44,
              106,107,119,121,135,156,157,158,160,162,165,167,168,170,171,172,173,174,175,176,177,179,180,181,182,
              208,
              211,216,217,218,219,210,
              220,221,222,223,224,225,226,227,
              242,243,244,
              292,293,294,295,296,297,298,299,
              288,
              321,308,
              423,444,445,446,447,448,449,450,451,455,
              731,732,733,734,735,
              895,896,
              1019,1033,
              1109,1111,
              2001,2002,2003,2004,2005,2006,2007,2008,
              2010,2011,2012,2013,2014,2015,2016,2017,2018,2019,
              2020,2021,2022,2023,2024,2025,2026,2027,2028,2029,
              2030,2031,2032,2033,2034,2035,2036,2037,2038,2039,
              2040,2041,2042,2043,
              2053,2054,2055,2056,2057,2058,2059,2060
              };// 修改地址需加密的设备


        public static int[] mintAllRelayDeviceType = new int[]{  // 所有继电器设备类型列表
           423,424,425,426,427,428,429,430,431,432,433,434,435,436,437,438,439,440,441,442,443,444,445,446,447,448,449,450,451,454,456,
        11,12,19,22,23,
        150,151,152,153};

        public static int[] minAllDimmerDeviceType = new int[]{ // 所有调光器设备类型列表
        1,2,3,4,5,6, 7,8, 9,13,14,15,17,36,37,40,42,43,44,45,46,
        25,26,27,28,30,31,35,41, 600,601,602,603,606,607,608,609,610,611,612,613,1015,852,614,615,616,617,
        618,619,620,621,455}; //混合模块界面特殊

        public static int[] minOnlyOnesequenceDeviceType = new int[] { 622,452,453,622,623,621,620,619,618,617,616,615,
        455,613,609,611,610,608,606,602,601,600,617,614,15,25,28,36};//只有一个序列的设备

        public static int[] mintHMIXDeviceType = new int[] {   //所有酒店混合模块的类型
        599 };

        public static int[] mintNewRelayFunction = new int[] { 423, 441, 442, 443, 444, 445, 446, 447, 448, 449, 450, 451, 454,457 };//楼梯灯

        public static int[] mintNewDimmerFunction = new int[] { 596,599,606,607,608,609,610,611,612,613,1015,852,614,615,616,617,
        618,619,620,621}; //调光曲线和温度

        public static int[] minAllWirelessDeviceType = new int[] { //所有无线设备
        1500,1501,
        5000,5001,5002,5003,
        5030, 5031, 5032, 5033, 5034, 5035, 5036, 5037, 5038, 5039, 5040, 5041, 
        5042,5043,5044,5045,5046,5047,5048,5049,5050,5051,5052,5053,5054,5055,5056,5057,5058,5059,5060,5061,
        5300,5301,5302,
        5500,5501,
        5700,5701,5702,
        5900,
        6100,6101,6102
        };

        public static int[] minAllPanelDeviceType = new int[]{  // 所有面板设备类型列表
            216,217,218,219,220,221,222,223,224,225,226,227,
        245,246,247,248,241,
        249,250,251,252,253,254,255,256,257,258,259,260,
        261,262,263,264,265,266,267,268,269,270,271,
        272,273,274,275,276,277,278,279,280,
        281,282,283,284,285,286,287,288,289,290,
        291,292,293,294,295,296,297,298,299,604,
        605,3050,3051,3052,3053,
        48,86,87,149,154,155,156,157,158,159,160,162,165,167,168,170,172,175,176,180,
        2001,2002,2003,2004,2005,2006,2007,2008,2010,2011,2012,2013,2014,2015,2016,2017,2018,2019,2020,2021,2022,2023,2024,2025,
        2026,2027,2028,2029,2030,
        2031,2032,2033,2034,2035,2036,2037,2038,2039,2040,2041,2042,2043,2044,2045,2046,2047,2048,2049,2050,2051,2052,2053,2054,
        2055,2056,2057,2058,2059,2060,2061,2062,2063,2064,2065,
        5000,5001,5002,5003,5004,
        5030, 5031, 5032, 5033, 5034, 5035, 5036, 5037, 5038, 5039, 5040, 5041, 
        5042,5043,5044,5045,5046,5047,5048,5049,5050,5051,5052,5053,5054,5055,5056,5057,5058,5059,5060,5061};

        public static int[] mintDLPDeviceType = new int[]{     // 所有DLP面板设备类型列表
            48,86,87,149,154,155,156,157,158,160,162,165,167,168,170,172,175,176,180,5000,5001,5002,5004,
        };
        public static int[] mintHotelACDeviceType = new int[] { 83, 85, 161, 166, 169, 178 };//酒店客房空调控制面板
        public static int[] mintNewDLPFHSetupDeviceType = new int[] { 162, 165, 167, 168, 170, 172, 175, 176, 180, 158, 5000, 5001, 5002, 5004,5005 };
        public static int[] mintMPTLDeviceType = new int[] { 165, 168, 170, 172, 175 };//十四按键DLP

        public static int[] mintWIFIPanelDeviceType = new int[] { 5000,5001,5002,5003,5004,5005, 
            5030, 5031, 5032, 5033, 5034, 5035, 5036, 5037, 5038, 5039, 5040, 5041, 
            5042,5043,5044,5045,5046,5047,5048,5049,5050,5051,5052,5053,5054,5055,5056,5057,5058,5059,5060,5061}; //wifi面板

        //public static int[] mintWIFIRoundButtonPanelDeviceType = new int[] { 5034, 5035, 5036, 5037, 5046, 5047, 5048, 5049, 5054 }; //wifi圆按键面板
        public static int[] mintWIFIThouchPanelDeviceType = new int[] { 5038, 5039, 5040, 5041, 5050, 5051, 5052, 5053, 5055, 5056, 5057, 5058, 5059, 5060, 5061 }; //wifi触摸面板
        public static int[] mintNewWIFIPanelDeviceType = new int[] { 5001, 5002,5003,5004,5005, 5042, 5043, 5044, 5045, 5046, 5047, 5048, 5049, 5050, 5051, 5052, 5053, 5054, 5055,5056,5057,
                                                                     5058,5059,5060,5061,5062,5063};//12无线回路，调光信息无线面板

        public static int[] mintAllIRDeviceType = new int[]{   // 所有红外发射设备类型列表
            313,300,301,302,303,304,306,319};

        public static int[] mintMotionDeviceType = new int[]{  // 所有红外动静传感器设备类型列表
        97,98,127,129,130,131, 132,133,138,139,95,99,120,122,123,125,126,127,
        80,90
        };

        public static int[] minMSPUDeviceType = new int[] { 317, 1500 };//红外超声波双鉴传感器


        public static int[] mintHVACDeviceType = new int[] { // 所有HVAC的设备类型列表
            106,107,111,112,730,731,732};
     

       

        public static int[] minLogicDeviceType = new int[] {   // 所有逻辑模块设备类型列表
            1100,1101,1102,1103,1104,1105,1106,1107,1108,1109};

        public static int[] minRs232DeviceType = new int[]{//所有rs232模块设备类型列表
        1007,1008,1009,1013,1014,1016,1017,1018,1019, 1020,1024,1027,1028,1029,1030,1031,1033,1037,1039,1042};

        public static int[] minMs04DeviceType = new int[] {//所有ms04设备类型
           113,114,115,116,119,118,121,93,141,140,142,135,136,137,351,352,353,354,355,356,357,358,359,360,361,362,363,
           1052,1053,
           5900};

        public static int[] minAudioDeviceType = new int[] { // 所有的背景音乐设备类型
            901,902,903,904,905,906,907, 913};

        public static int[] mintSecurityType = new int[] {  //安防模块设备类型列表
            3047,3049};

        public static int[] minMHRCUType = new int[] { 3501, 3502, 3503 };//酒店主机设备类型

        public static int[] minMPCOType = new int[] { 5302 };//一路窗帘驱动器

        public static int[] minMPROType = new int[] { 5500, 5501 };//继电器驱动器

        public static int[] minMPDOType = new int[] { 5700, 5701, 5702 };//调光驱动器

        public static int[] intConsoleDevType = new int[]
        {
            306,315,                                                 // IR
            313,302,300,301,303,304,
            424,427,428,429,430,431,432,433,434,435,436,437,438,439,440,
            11,12,19,22,23,423,425,426,441,442,443,
            150,151,152,153,600,601,602,7,9,27,40,21,17,
            1,2,3,4,5,6,8,13,14,15,16,18,20,32,
            25,26,28,30,31,35,41,606,607,608,609,850,1015,851,852,
            134,124,700,701,702,703,704,705,706,
            900,901,902,903,904,905,
            106,107,
            599
        };  // 被控制的设备DeviceType

       

        public static int[] mintHAIDeviceType = new int[] // 所有HAI的设备类型
        {
            950
        };

        public static int[] mintEIBDeviceType = new int[] // 所有EIB的设备类型
        {
            1001,1050,1051
        };

        public static int[] mintSPADeviceType = new int[] { 163 }; // 所有SPA控制器类型       

        public static int[] mintLEDLightsDeviceType = new int[] { 201 }; // 所有舞台LED灯

        public static int[] mintWIFIDeviceType = new int[] { 740,741,742, 
            1500,1501,1502,
            5000,5001,5002, 5003,5004,
            5030, 5031, 5032, 5033, 5034, 5035, 5036, 5037, 5038, 5039, 5040, 5041,
            5042,5043,5044,5045,5046,5047,5048,5049,5050,5051,5052,5053,5054,5055,5056,5057,5058,5059,5060,5061,
            5300,5301, 5302,
            5500,5501,
            5700,5701,5702,
            5900,
            6100,6101,6102,
        }; //所有的wifi模块

        public static int[] mintMeshDeviceType = new int[] { 740, 741, 742 };//所有无线网关设备

        public static int[] mintDevicesHasIPNetworkDevType = new int[] { 1013, 1014, 1018, 1023, 1024, 1027, 1037, 908, 1600}; //含有网络信息数据的设备

        public static int[] minBackNetDeviceType = new int[] { 1023 };////BackNet 设备类型

        public static int[] minSameControlOfDryCont = new int[] { 3501, 3502, 3503, 5302, 5500, 5501, 5700, 5701, 5702, 5900, 6100 };

        public static int[] minCoolMasterNetDeviceType = new int[] { 953 };//带网口CoolMaster
        public static int[] minCoolMasterDeviceType = new int[] { 951 };//CoolMaster
        public static int[] minDoorBellDeviceType = new int[] { 3054, 3056, 3059, 3070, 3071, 3072, 3073, 3074, 3075, 3077, 3081 };//门铃
        public static int[] minCardReaderDeviceType = new int[] { 3057, 3058, 3062, 3063, 3064, 3065, 3066, 3068, 3069, 3076, 3078, 3080 };//插卡取电
        public static int[] minMSPUBRFDeviceType = new int[] { 1501 };//电池装红外传感器
        public static int[] minReceptaclesDeviceType = new int[] { 6102 };//无线插座
        public static int[] NetworkInformationHasDhcp = new int[] { 231, 232, 233, 234, 742, 9000, 9001 }; // DHCP
        public static int[] minFANControllerDeviceType = new int[] { 738 };//风扇控制器
        public static int[] minBrazilWirelessPanelDeviceType = new int[] { 1800 };//巴西面板
        public static int[] minSensor_7in1DeviceType = new int[] { 312, 328 };//7合一

        public static int[] SonosConvertorDeviceType = new int[] { 36866,51966}; // sonos convertor

        public static int mintMaxTemperature = 4;
        #endregion

        public static int[] KeManLiDeviceTypeList = new int[] {171,173,174,177,179,181,182,
                                                                  740,741,742,743,744,
                                                                  1037,1042,
                                                                  1500,
                                                                  5056,5060,5062,5063,5064,
                                                                  5300,5302,5303,
                                                                  5500,5501,5502,5503,
                                                                  5700,5702,
                                                                  5900,
                                                                  6100};//柯曼丽设备类型

        public static Boolean isKamanli = false;

        public static String[] sLightSmallTypesList = new String[] { "Dimming Channel","Relay","CCT","RGB","RGBW","RGBW + CCT","DALI","Logic Light"};

        public static String[] sCurtainSmallTypesList = new String[] { "Curtain Motor", "Shutter Moter", "Curtain Controller" };

        public static String[] sButtonSmallTypesList = new String[] { "Dry Contact", "Button"};

        public static String[] sSensorSmallTypesList = new String[] { "Third Part Sensor", "PIR", "Temperature Sensor", "Humidity Sensor", "Lux Sensor", "VOC", "PM2.5", "CO2" };

        #region
        public static String[] LoadType = null; //负载类型
        public static String[] Weekdays =null; // 星期几
        public static String[] Status = null;  // 机械开关的开和关
        public static String[] UvSwitchStatus = null;  //通用开关的开和关
        public static String[] StatusAC = null;   //空调状态
        public static String[] StatusFAN = null;   // 风速
        public static List<String> CurtainModes = null;  // 风机状态
        public static String[] CurtainRelayState = null ;
        public static String[] NewIRLibraryDeviceType = null; // 红外码库类型列表
        public static String[] strWirelessAddressState = null;
        public static String[] strWirelessAddressConnection = null;
        public static String[] security = null;
        public static String[] MusicControl = null;
        public static String[] GPRSControl = null; // GPRS 
        public static string[] PanelControl = null; // 面板控制类型
        #endregion

        public static string[] EIBConverTors = new string[]{"Scene(1 byte)","Scene Dimmer(4 bit)","Sequence(1 byte)","Universal switch(1 bit)","Single channel switch(1 bit)",
                   "Single channel Dimmer(4 bit)","Broadcast scene(1 byte)","Broadcast Channels Switch(1 bit)","Broadcast Channels Dimmer(4 bit)","Curtain switch on(1 bit)",
                   "Curtain switch off(1 bit)","Message(1 byte)","String Conversion(14 byte)","Absolute Dimming(1 byte)","Current Temperature(2 byte)"};

        public static string[] strAryCMD = new string[] { "自定义", "场景控制", "序列控制", "单路调节", "通用开关", "窗帘开关", "GPRS控制" };

        public static string[] BackNetIDString = new string[] { "AI", "AO", "AV", "BI", "BO", "BV", "MI", "MO", "MV", "Others" };
        public static string[] LogicHints = new string[] { "AI", "AO", "When", "Date:", "When", "Time", "Its uv switch", "degree in", "run scene", "run seq.", "uv switch", "chn no.", "curtain ", "panel ", "area " };
              
        public static string[] strAryRS232Time = new string[] { "100(MS)", "300(MS)", "500(MS)", "1000(MS)", "2000(MS)", "3000(MS)", "4000(MS)", "5000(MS)" };
        public static string MyUnnamed = "<UNNAMED>";

        public static List<LoadControlsText.FormDisplayTextList> WholeTextsList; // 整个完整的列表  
        public static List<LoadControlsText.FormsInformation> FormControlsIdinfo;///各个窗体的信息  

        public static string mstrInvalid = "N/A";

        public static string mstrDefaultPath = null;  // default blank database path

        public static string mstrCurPath = null;  //当前工程的地址所在位置

        public static string mstrINIDefaultPath = null; // 读取配置显示文件的位置

        public static List<byte[]> MyUPload2DownLists = null; //表示上传或者下载设备列表 子网ID 设备ID 设备类型  
        public static byte MyUpload2Down = 0; // 0 下载  1 上传

        public static int AutoAddress = 0;

        public static int MyDimmerTemplate = 0; // 0 HDL default database, 1 norway database

        public static int iIsDragTemplateOrSimpleListOrCmd = -1;  // 0 1, 2

        /// <summary>
        /// 数据通讯时参数
        /// </summary>
        #region
        public static string myLocalIP = null; // 本机地址
        public static string myDestIP = null;  //发送到的IP地址
        public static int myintProxy = 0; // 0 局域网，1 点对点  2， 通过serverDown
        public static Boolean bIsAutoUpgrade = false;//是不是自动更新
        public static Boolean bIsDownloadSuccess = false;


        public static HDLSend mySends = new HDLSend();					//建立发送机制
        public static byte[] myRevBuf = null;

        public static byte[] ArayLevel = null;

        public static List<byte[]> MyQuene = null;//命令测试存储数据
        public static List<byte[]> MySimpleSearchQuene = null; //temperature buffer for them 

        public static List<byte[]> MySimpleDeviceListQuene = null; // save then could easy to change part information

        public static bool MyBlnModifyNetModule = false;
        public static bool MyBlnStartSimpleSearch = false;

        public static int MyUpgradeSubNetID = -1;
        public static int MyUpgradeDeviceID = -1;

        /// <summary>
        /// 重放相隔次数
        /// </summary>
        public static int replySpanTimes = 2000;//重发延时	
        public static int MoreDelay = 0;//重发再延时
        public static int mintFactoryTime = -1;//恢复出场设置计时
        /// <summary>
        /// 重放数据次数
        /// </summary>
        public static int replytimes = 4;//重发次数

        public static bool BigPacket = false;       //大包协议发送

         // 收发数据时 参数一致认为是正常
        public static bool MyBlnNeedF8 = false; // 是不是返回F8 
        public static int  MybytNeedParm1 = -1; // 第一个参数
        public static int  MybytNeedParm2 = -1; // 第二个参数
        public static int  MybytNeedParm3 = -1; // 第三个参数
        public static List<int> NeedAppendToPublicBufferCommands = new List<int> { } ; //0x1365};

        /// <summary>
        /// 数据上传模式 全部上传 0   分页上传1   手动上传2
        /// </summary>
        public static byte myUploadMode = 0;
        /// <summary>
        /// 数据下载模式 全部下载 0   分页下载1   手动下载2
        /// </summary>
        public static byte myDownLoadMode = 0;
        #endregion

        public static Boolean isIniAllKeySucess; 

        public static BackgroundWorker calculationWorker = null;

        public static byte mbytCurUsingSubNetID = 255;
        public static byte mbytCurUsingDevNetID = 255;
        public static byte mbytLocalSubNetID = 12;
        public static byte mbytLocalDeviceID = 254;
        public static int mintLocalDeviceType = 0xFFFE;
        public static bool MyBlnWait15FE = false;
        public static bool MyBlnCapture = false;  //表示是不是开始捕捉数据
        public static int MyMaxPacket = -1;  // 升级可以容纳的最大包数
        public static int MyUPgradeDeviceType = -1; // 升级设备的设备类型
        public static int MyStartORAskMore = -1; // 开始升级获取发送次数

        //获取当前活动的IP
        public static List<string> mstrActiveIPs;


        /// <summary>
        /// 工程模式 还是 工程师模式
        /// </summary>
        public static byte MyEditMode = 1;

        public static Boolean myUploadDownManually = false;

        public static bool MyShowWizardorNot = true;

        public static bool MyStartImport = false; // import from database or just change online 
        public static Color MyImportColor = new System.Drawing.Color();
        public static List<Color> MyImportColorList = null;
        public static List<int> MyImportDIndex = null;
        public static int ImportwdDeviceType = -1;

        public static bool MyStartOnlineMatch = false; // match from online devices list
        public static Color MyOnlineColor = new System.Drawing.Color();
        public static List<Color> MyOnlineColorList = null;
        public static List<int> MyOnlineDIndex = null;
        public static int OnlinewdDeviceType = -1;
        public static string MyTmpOnlineDevice = string.Empty;
        public static string MyMachineCodeMac = string.Empty;
        public static bool FastSearch = false;    // 是否表示全局搜索
        public static bool WaitMore = false;

        public static List<DevOnLine> myOnlines = new List<DevOnLine>(); // 表示当前在线设备列表

        public static List<DevOnLine> panelWaitUpload = new List<DevOnLine>(); // 需要上传图片的

        // 2014 03 11 New add
        public static List<String> MyTmpName = null;

        /// <summary>
        /// // 0 表示传统模式  1 表示简易编辑模式
        /// </summary>
        public static int MyProgramMode = 0;
        public static int MyEnterProjectWay = 0; // 本地或者远程

        public static int mintCurRmID = -1;  //楼层列表
        public static int mintFHID = -1;  // 房间ID
        public static String softwareverson = "HDL Buspro Setup Tool 2 V03.07B";
        public static int iLanguageId = 0; 
        // 0 = English 
        // 1 = Chinese

        public static Boolean MyBlnReRead = false; //是不是要重新搜索

        public static Boolean MyBlnFinish = false;
        public static Byte CurrentUpgradeSubnetID = 0;
        public static Byte CurrentUpgradeDeviceID = 0;
        public static Byte UpgradSubnetIDForAuto = 0;
        public static Byte UpgradDeviceIDForAuto = 0;
        public static Byte ModifyDeviceAddressSubNetID = 0;
        public static Byte ModifyDeviceAddressDeviceID = 0;
        public static Boolean isRestore = false;
        public static Boolean isBackUpSucceed = false;
        public static bool isStartUploadFile = false;
        public static String RestoreRemark = "";

        public static int ChannelIDForNewIR = 0;
        public static int KeyIDForNewIR = 0;
        public static byte[] arayACParamForNewIR = new byte[6];
        public static int DownloadPictruePageID = 1;

        public static bool isWriteDataToUSB = true;
        public static bool isAutoRefreshCurtainPercent = false;
        public static bool isRightPasswork = false;//校准密码

        public static int UploadColorDLPType = 1;
        public static bool isStopDealImageBackground = false;


        public static List<DimTemplates> myDimTemplates = null; // 所有调光曲线的素材库
        public static List<ControlTemplates> myTemplates = null;
        public static List<ButtonMode> myPublicButtonMode = null;   //公共按键模式保存地方
        public static List<ButtonControlType> myPublicControlType = null; // 公共目标保存地方
        public static List<DryMode> myPublicDryMode = null; // 公共干结点保存地方
        public static List<DryControlType> myDryGroupControlType = null; // 公共目标保存地方
        public static List<DeviceTypeList> myDeviceTypeLists;  // 将所有设备类型读取到数据库
        public static List<HdlDeviceBackupAndRestore> myBackupLists; // 所有用于备份和恢复的数据库

        //public devices structs
        public static List<IPModule> myIPs = null;
        public static List<Relay> myRelays = null;
        public static List<Dimmer> myDimmers = null;
        public static List<BacNet> myBacnets = null;
        public static List<Panel> myPanels = null; // 普通面板
        public static List<Camera> myCameras = null; // 摄像头或者
        public static List<ColorDLP> myColorPanels = null;//表示所有的彩色DLP
        public static List<EnviroPanel> myEnviroPanels = null; // 新版面板
        public static List<CoolMaster> myCoolMasters = null; // coolmaster
        public static List<MS04> myDrys = null; // 所有MS04
        public static List<Curtain> myCurtains = null; // 所有的窗帘模块
        public static List<HAI> myHais = null; // 所有的HAI模块
        public static List<HVAC> myHvacs = null; // 所有的HVAC 模块
        public static List<FH> myFHs = null; // 所有的FH模块
        public static List<DMX> myDmxs = null; // 所有的FH模块
        public static List<RFSocket> mySockets = null; // 所以无线模块
        public static List<MSPU> myPUSensors = null; // 所有超声波传感器
        public static List<Security> mySecurities = null; // 所有的安防模块
        public static List<MultiSensor> mysensor_8in1 = null; // 所有8in1
        public static List<Sensor_12in1> mysensor_12in1 = null; // 所有12in1
        public static List<Sensor_7in1> mysensor_7in1 = null; // 所有7in1
        public static List<MiniSensor> myMiniSensors = null; // 所有迷你传感器
        public static List<MzBox> myzBoxs = null; // 所有的音乐播放器
        public static List<Logic> myLogics = null; // 所有的逻辑模块
        public static List<MMC> myMediaBoxes = null; // 所有多媒体播放器
        public static List<MHRCU> myRcuMixModules = null; // 所有酒店混合模块
        public static List<MHIC> myCardReader = null; // 所有插卡取电模块
        public static List<TempSensor> myTemperatureSensors = null; // 所有温度传感器列表
        public static List<DS> myDS = null;//表示所有十寸屏室外机
        public static List<NewDS> myNewDS = null;//表示所有新的门口机
        public static List<NewIR> myNewIR = null;//表示所有的新型红外发射
        public static List<RS232> myRS232 = null; // 表示所有的232模块
        public static List<GPRS> myGPRS = null;   // 存放所有的GPRS模块


        public static List<UVCMD.ControlTargets> MyPublicCtrls = null; //存放公共的目标
        public static List<Object[]> MyCopyDataGridView = null; // 复制表格内容
        public static List<DeviceInfo> MyTmpSensors = null; // 传感器公共复制
        public static List<Object[]> RowObj = null; //复制表格数据
        public static List<LEDLibray> myLEDs = null;
        public static UVCMD.DeviceAllIRInfo[] MyLibrary = null;

        //0 : 温度; 1 干结点； 2：HVAC 3: 按键 干结点 ; 255 ： 全部
        public const Byte TemperatureGroup = 0;
        public const Byte DryContactGroup = 1;
        public const Byte HvacGroup = 2;
        public const Byte ButtonSetupGroup = 3;

        public static String[] mstrDALISuccessEng = new String[6] {"Normal","Normal","OFF","In","Dimming Complete","NO"};
        public static String[] mstrDALIFailEng = new String[6] { "Fault", "Fault", "ON", "Out", "Dimming in Progress", "YES" };

        public static String[] mstrDALISuccess= new String[6] {"正常","正常","关","范围内","调光已完成","否"};
        public static String[] mstrDALIFail= new String[6] {"损坏","损坏","开","范围外","调光中","是"};

        public const Byte AllDeviceGroup = 255;

        public static Int32 SensorMixModuleTotalLogicBlock = 24;
        public static Int32 sumCommandsInSensorMixModuleEveryBlock = 10;

        ///简易编程部分
        ///
        #region
        public static List<DimmerChannel> simpleSetupDimmerChns = null;
        public static List<RelayChannel> simpleSetupRelayChns = null;
        public static List<HdlDevice> simpleSearchDevicesList = null;

        public static Boolean bStartSimpleTesting = false;

        #endregion

    }
}
