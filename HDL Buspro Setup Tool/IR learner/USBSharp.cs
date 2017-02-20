using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace HDL_Buspro_Setup_Tool
{
    /// <summary>
    /// Summary description
    /// </summary>
    public class USBSharp
    {

        #region Structs and DLL-Imports
        //
        //
        // Required constants, pointers, handles and variables 
        public int HidHandle = -1;				// file handle for a Hid devices
        public int hDevInfo = -1;				// handle for the device infoset
        public string DevicePathName = "";
        public const int DIGCF_PRESENT = 0x00000002;
        public const int DIGCF_DEVICEINTERFACE = 0x00000010;
        public const int DIGCF_INTERFACEDEVICE = 0x00000010;
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const int OPEN_EXISTING = 3;
        public const int EV_RXFLAG = 0x0002;    // received certain character

        // specified in DCB
        public const int INVALID_HANDLE_VALUE = -1;
        public const int ERROR_INVALID_HANDLE = 6;
        public const int FILE_FLAG_OVERLAPED = 0x40000000;

        // GUID structure
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct GUID
        {
            public int Data1;
            public System.UInt16 Data2;
            public System.UInt16 Data3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] data4;
        }

        // Device interface data
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public GUID InterfaceClassGuid;
            public int Flags;
            public int Reserved;
        }

        // Device interface detail data
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public unsafe struct PSP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        // HIDD_ATTRIBUTES
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct HIDD_ATTRIBUTES
        {
            public int Size; // = sizeof (struct _HIDD_ATTRIBUTES) = 10

            //
            // Vendor ids of this hid device
            //
            public System.UInt16 VendorID;
            public System.UInt16 ProductID;
            public System.UInt16 VersionNumber;

            //
            // Additional fields will be added to the end of this structure.
            //
        }

        // HIDP_CAPS
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct HIDP_CAPS
        {
            public System.UInt16 Usage;					// USHORT
            public System.UInt16 UsagePage;				// USHORT
            public System.UInt16 InputReportByteLength;
            public System.UInt16 OutputReportByteLength;
            public System.UInt16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public System.UInt16[] Reserved;				// USHORT  Reserved[17];			
            public System.UInt16 NumberLinkCollectionNodes;
            public System.UInt16 NumberInputButtonCaps;
            public System.UInt16 NumberInputValueCaps;
            public System.UInt16 NumberInputDataIndices;
            public System.UInt16 NumberOutputButtonCaps;
            public System.UInt16 NumberOutputValueCaps;
            public System.UInt16 NumberOutputDataIndices;
            public System.UInt16 NumberFeatureButtonCaps;
            public System.UInt16 NumberFeatureValueCaps;
            public System.UInt16 NumberFeatureDataIndices;
        }

        //HIDP_REPORT_TYPE 
        public enum HIDP_REPORT_TYPE
        {
            HidP_Input,		// 0 input
            HidP_Output,	// 1 output
            HidP_Feature	// 2 feature
        }

        // Structures in the union belonging to HIDP_VALUE_CAPS (see below)

        // Range
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Range
        {
            public System.UInt16 UsageMin;			// USAGE	UsageMin; // USAGE  Usage; 
            public System.UInt16 UsageMax; 			// USAGE	UsageMax; // USAGE	Reserved1;
            public System.UInt16 StringMin;			// USHORT  StringMin; // StringIndex; 
            public System.UInt16 StringMax;			// USHORT	StringMax;// Reserved2;
            public System.UInt16 DesignatorMin;		// USHORT  DesignatorMin; // DesignatorIndex; 
            public System.UInt16 DesignatorMax;		// USHORT	DesignatorMax; //Reserved3; 
            public System.UInt16 DataIndexMin;		// USHORT  DataIndexMin;  // DataIndex; 
            public System.UInt16 DataIndexMax;		// USHORT	DataIndexMax; // Reserved4;
        }

        // Range
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NotRange
        {
            public System.UInt16 Usage;
            public System.UInt16 Reserved1;
            public System.UInt16 StringIndex;
            public System.UInt16 Reserved2;
            public System.UInt16 DesignatorIndex;
            public System.UInt16 Reserved3;
            public System.UInt16 DataIndex;
            public System.UInt16 Reserved4;
        }
        //HIDP_VALUE_CAPS
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
        public unsafe struct HIDP_VALUE_CAPS
        {
            //
            [FieldOffset(0)]
            public System.UInt16 UsagePage;					// USHORT
            [FieldOffset(2)]
            public System.Byte ReportID;						// UCHAR  ReportID;
            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(3)]
            public System.Boolean IsAlias;						// BOOLEAN  IsAlias;
            [FieldOffset(4)]
            public System.UInt16 BitField;						// USHORT  BitField;
            [FieldOffset(6)]
            public System.UInt16 LinkCollection;				//USHORT  LinkCollection;
            [FieldOffset(8)]
            public System.UInt16 LinkUsage;					// USAGE  LinkUsage;
            [FieldOffset(10)]
            public System.UInt16 LinkUsagePage;				// USAGE  LinkUsagePage;
            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(12)]
            public System.Boolean IsRange;					// BOOLEAN  IsRange;
            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(13)]
            public System.Boolean IsStringRange;				// BOOLEAN  IsStringRange;
            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(14)]
            public System.Boolean IsDesignatorRange;			// BOOLEAN  IsDesignatorRange;
            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(15)]
            public System.Boolean IsAbsolute;					// BOOLEAN  IsAbsolute;
            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(16)]
            public System.Boolean HasNull;					// BOOLEAN  HasNull;
            [FieldOffset(17)]
            public System.Char Reserved;						// UCHAR  Reserved;
            [FieldOffset(18)]
            public System.UInt16 BitSize;						// USHORT  BitSize;
            [FieldOffset(20)]
            public System.UInt16 ReportCount;					// USHORT  ReportCount;
            [FieldOffset(22)]
            public System.UInt16 Reserved2a;					// USHORT  Reserved2[5];		
            [FieldOffset(24)]
            public System.UInt16 Reserved2b;					// USHORT  Reserved2[5];
            [FieldOffset(26)]
            public System.UInt16 Reserved2c;					// USHORT  Reserved2[5];
            [FieldOffset(28)]
            public System.UInt16 Reserved2d;					// USHORT  Reserved2[5];
            [FieldOffset(30)]
            public System.UInt16 Reserved2e;					// USHORT  Reserved2[5];
            [FieldOffset(32)]
            public System.UInt16 UnitsExp;					// ULONG  UnitsExp;
            [FieldOffset(34)]
            public System.UInt16 Units;						// ULONG  Units;
            [FieldOffset(36)]
            public System.Int16 LogicalMin;					// LONG  LogicalMin;   ;
            [FieldOffset(38)]
            public System.Int16 LogicalMax;					// LONG  LogicalMax
            [FieldOffset(40)]
            public System.Int16 PhysicalMin;					// LONG  PhysicalMin, 
            [FieldOffset(42)]
            public System.Int16 PhysicalMax;					// LONG  PhysicalMax;
            // The Structs in the Union			
            [FieldOffset(44)]
            public Range Range;
            [FieldOffset(44)]
            public Range NotRange;
        }


        // ----------------------------------------------------------------------------------
        //
        //
        //
        // 
        // Define istances of the structures
        //
        //

        private Guid MYguid = new Guid();
        //
        // SP_DEVICE_INTERFACE_DATA  mySP_DEVICE_INTERFACE_DATA = new SP_DEVICE_INTERFACE_DATA();
        //
        public SP_DEVICE_INTERFACE_DATA mySP_DEVICE_INTERFACE_DATA;
        //
        public PSP_DEVICE_INTERFACE_DETAIL_DATA myPSP_DEVICE_INTERFACE_DETAIL_DATA;
        // 
        public HIDD_ATTRIBUTES myHIDD_ATTRIBUTES;
        //
        public HIDP_CAPS myHIDP_CAPS;
        //
        public HIDP_VALUE_CAPS[] myHIDP_VALUE_CAPS;

        // ******************************************************************************
        // DLL Calls
        // ******************************************************************************

        //Get GUID for the HID Class
        [DllImport("hid.dll", SetLastError = true)]
        static extern unsafe void HidD_GetHidGuid(
            ref Guid lpHidGuid);

        //Get array of structures with the HID info
        [DllImport("setupapi.dll", SetLastError = true)]
        static extern unsafe int SetupDiGetClassDevs(
            ref Guid lpHidGuid,
            int* Enumerator,
            int* hwndParent,
            int Flags);


        //Get context structure for a device interface element
        //
        //  SetupDiEnumDeviceInterfaces returns a context structure for a device 
        //  interface element of a device information set. Each call returns information 
        //  about one device interface; the function can be called repeatedly to get information 
        //  about several interfaces exposed by one or more devices.
        //
        [DllImport("setupapi.dll", SetLastError = true)]
        static extern unsafe int SetupDiEnumDeviceInterfaces(
            int DeviceInfoSet,
            int DeviceInfoData,
            ref  Guid lpHidGuid,
            int MemberIndex,
            ref  SP_DEVICE_INTERFACE_DATA lpDeviceInterfaceData);


        //[DllImport("setupapi.dll",SetLastError=false)]
        //    static extern unsafe bool SetCommTimeouts(int hCommport
        //	Get device Path name
        //  Works for the first pass  --> to get the required size

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern unsafe int SetupDiGetDeviceInterfaceDetail(
            int DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA lpDeviceInterfaceData,
            int* aPtr,
            int detailSize,
            ref int requiredSize,
            int* bPtr);

        //	Get device Path name
        //  Works for second pass (overide), once size value is known

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern unsafe int SetupDiGetDeviceInterfaceDetail(
            int DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA lpDeviceInterfaceData,
            ref PSP_DEVICE_INTERFACE_DETAIL_DATA myPSP_DEVICE_INTERFACE_DETAIL_DATA,
            int detailSize,
            ref int requiredSize,
            int* bPtr);

        // Get Create File
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int CreateFile(
            string lpFileName,							// file name
            uint dwDesiredAccess,						// access mode
            uint dwShareMode,							// share mode
            uint lpSecurityAttributes,					// SD
            uint dwCreationDisposition,					// how to create
            uint dwFlagsAndAttributes,					// file attributes
            uint hTemplateFile							// handle to template file
            );


        [DllImport("hid.dll", SetLastError = true)]
        private static extern int HidD_GetAttributes(
            int hObject,								// IN HANDLE  HidDeviceObject,
            ref HIDD_ATTRIBUTES Attributes);			// OUT PHIDD_ATTRIBUTES  Attributes


        [DllImport("hid.dll", SetLastError = true)]
        private unsafe static extern int HidD_GetPreparsedData(
            int hObject,								// IN HANDLE  HidDeviceObject,
            ref int pPHIDP_PREPARSED_DATA);				// OUT PHIDP_PREPARSED_DATA  *PreparsedData


        [DllImport("hid.dll", SetLastError = true)]
        private unsafe static extern int HidP_GetCaps(
            int pPHIDP_PREPARSED_DATA,					// IN PHIDP_PREPARSED_DATA  PreparsedData,
            ref HIDP_CAPS myPHIDP_CAPS);				// OUT PHIDP_CAPS  Capabilities

        [DllImport("hid.dll")]
        public unsafe static extern bool HidD_SetOutputReport(int HidDeviceObject, ref byte lpReportBuffer, int ReportBufferLength);


        [DllImport("hid.dll", SetLastError = true)]
        private unsafe static extern int HidP_GetValueCaps(
            HIDP_REPORT_TYPE ReportType,								// IN HIDP_REPORT_TYPE  ReportType,
            [In, Out] HIDP_VALUE_CAPS[] ValueCaps,						// OUT PHIDP_VALUE_CAPS  ValueCaps,
            ref int ValueCapsLength,									// IN OUT PULONG  ValueCapsLength,
            int pPHIDP_PREPARSED_DATA);									// IN PHIDP_PREPARSED_DATA  PreparsedData



        [DllImport("kernel32.dll", SetLastError = true)]
        private unsafe static extern bool ReadFile(
            int hFile,						// handle to file
            ref byte[] lpBuffer,				// data buffer
            int nNumberOfBytesToRead,		// number of bytes to read
            ref int lpNumberOfBytesRead,	// number of bytes read
            int* ptr
            // 
            // ref OVERLAPPED lpOverlapped		// overlapped buffer
            );

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern unsafe int SetupDiDestroyDeviceInfoList(
            int DeviceInfoSet				// IN HDEVINFO  DeviceInfoSet
            );

        // 13
        [DllImport("hid.dll", SetLastError = true)]
        static extern unsafe int HidD_FreePreparsedData(
            int pPHIDP_PREPARSED_DATA			// IN PHIDP_PREPARSED_DATA  PreparsedData
            );



        // API declarations relating to file I/O.

        // ******************************************************************************
        // API constants
        // ******************************************************************************


        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        public const int WAIT_TIMEOUT = 0x102;
        public const short WAIT_OBJECT_0 = 0;

        // ******************************************************************************
        // Structures and classes for API calls, listed alphabetically
        // ******************************************************************************

        [StructLayout(LayoutKind.Sequential)]
        public struct OVERLAPPED
        {
            public int Internal;
            public int InternalHigh;
            public int Offset;
            public int OffsetHigh;
            public int hEvent;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public int lpSecurityDescriptor;
            public int bInheritHandle;
        }


        [DllImport("kernel32.dll")]
        static public extern int CancelIo(int hFile);

        [DllImport("kernel32.dll")]
        static public extern int CloseHandle(int hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static public extern int CreateEvent(ref SECURITY_ATTRIBUTES SecurityAttributes, int bManualReset, int bInitialState, string lpName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static public extern int
          CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, ref SECURITY_ATTRIBUTES lpSecurityAttributes, int dwCreationDisposition, uint dwFlagsAndAttributes, int hTemplateFile);

        [DllImport("kernel32.dll")]
        static public extern int ReadFile(int hFile, ref byte lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, ref OVERLAPPED lpOverlapped);

        [DllImport("kernel32.dll")]
        static public extern int WaitForSingleObject(int hHandle, int dwMilliseconds);

        [DllImport("kernel32.dll")]
        static public extern int WriteFile(int hFile, ref byte lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, int lpOverlapped);


        [DllImport("kernel32.dll")]
        static public extern int ReadFile(int hFile, ref byte lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, int lpOverlapped);



        #endregion

        //	--------------------------------------
        // Managed Code wrappers for the DLL Calls
        // Naming convention ---> same as unmaneged DLL call with prefix CT_xxxx 
        //---#+************************************************************************
        //---NOTATION:
        //-  CT_HidGuid()
        //-
        //--- DESCRIPTION:
        //--  GUID for HID
        //                                                             Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe void CT_HidGuid()
        {
            HidD_GetHidGuid(ref MYguid);	// 
        }

        /// <summary>
        /// 打开写文件句柄
        /// </summary>
        /// <param name="devicePath"></param>
        /// <returns>句柄</returns>
        public unsafe int CreateWriteFile(string devicePath)
        {
            return CreateFile(
                    devicePath,
                    GENERIC_WRITE | GENERIC_READ,
                    FILE_SHARE_READ | FILE_SHARE_WRITE,
                    0,
                    OPEN_EXISTING,
                    0,
                    0);
        }

        /// <summary>
        /// 打开读文件句柄
        /// </summary>
        /// <param name="devicePath"></param>
        /// <returns></returns>
        public unsafe int CreateReadFile(string devicePath)
        {
            return CreateFile(
                devicePath,
                GENERIC_WRITE | GENERIC_READ,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                0,
                OPEN_EXISTING,
                0,
                // FILE_FLAG_OVERLAPPED,
                0);

        }
        public unsafe string CT_HidGuid1()
        {
            HidD_GetHidGuid(ref MYguid);
            if (MYguid.ToString() != null)
            {
                return MYguid.ToString();
            }
            else
                return null;
        }
        //---#+************************************************************************
        //---NOTATION:
        //-  CT_SetupDiGetClassDevs()
        //-
        //--- DESCRIPTION:
        //--  
        //                                                             Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe int CT_SetupDiGetClassDevs()
        {
            hDevInfo = SetupDiGetClassDevs(
                ref MYguid,
                null,
                null,
                DIGCF_INTERFACEDEVICE | DIGCF_PRESENT);
            return hDevInfo;
        }

        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_SetupDiEnumDeviceInterfaces(int memberIndex)
        //-
        //--- DESCRIPTION:
        //--  
        //                                                             Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe int CT_SetupDiEnumDeviceInterfaces(int memberIndex)
        {
            mySP_DEVICE_INTERFACE_DATA = new SP_DEVICE_INTERFACE_DATA();
            mySP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(mySP_DEVICE_INTERFACE_DATA);
            int result = SetupDiEnumDeviceInterfaces(
                hDevInfo,
                0,
                ref  MYguid,
                memberIndex,
                ref mySP_DEVICE_INTERFACE_DATA);
            return result;
        }


        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_SetupDiGetDeviceInterfaceDetail(ref int RequiredSize, int DeviceInterfaceDetailDataSize)
        //-
        //--- DESCRIPTION:
        //--    results = 0 is OK with the first pass of the routine since we are
        //-     trying to get the RequiredSize parameter so in the next call we can read the entire detail 
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe int CT_SetupDiGetDeviceInterfaceDetail(ref int RequiredSize, int DeviceInterfaceDetailDataSize)
        {
            int results =
            SetupDiGetDeviceInterfaceDetail(
                hDevInfo,							// IN HDEVINFO  DeviceInfoSet,
                ref mySP_DEVICE_INTERFACE_DATA,		// IN PSP_DEVICE_INTERFACE_DATA  DeviceInterfaceData,
                null,								// OUT PSP_DEVICE_INTERFACE_DETAIL_DATA  DeviceInterfaceDetailData,  OPTIONAL
                DeviceInterfaceDetailDataSize,		// IN DWORD  DeviceInterfaceDetailDataSize,
                ref RequiredSize,					// OUT PDWORD  RequiredSize,  OPTIONAL
                null); // 
            return results;
        }

        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_SetupDiEnumDeviceInterfaces(int memberIndex)
        //-
        //--- DESCRIPTION:
        //--    results = 1 iin the second pass of the routine is success
        //-     DeviceInterfaceDetailDataSize parameter (RequiredSize) came from the first pass
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe int CT_SetupDiGetDeviceInterfaceDetailx(ref string devicePath, ref int RequiredSize, int DeviceInterfaceDetailDataSize)
        {
            myPSP_DEVICE_INTERFACE_DETAIL_DATA = new PSP_DEVICE_INTERFACE_DETAIL_DATA();
            myPSP_DEVICE_INTERFACE_DETAIL_DATA.cbSize = 5;
            int results =
                SetupDiGetDeviceInterfaceDetail(
                hDevInfo,									// IN HDEVINFO  DeviceInfoSet,
                ref mySP_DEVICE_INTERFACE_DATA,				// IN PSP_DEVICE_INTERFACE_DATA  DeviceInterfaceData,
                ref myPSP_DEVICE_INTERFACE_DETAIL_DATA,		// DeviceInterfaceDetailData,  OPTIONAL
                DeviceInterfaceDetailDataSize,				// IN DWORD  DeviceInterfaceDetailDataSize,
                ref RequiredSize,							// OUT PDWORD  RequiredSize,  OPTIONAL
                null); // 
            DevicePathName = myPSP_DEVICE_INTERFACE_DETAIL_DATA.DevicePath;
            devicePath = myPSP_DEVICE_INTERFACE_DETAIL_DATA.DevicePath;
            return results;
        }

        //---#+************************************************************************
        //---NOTATION:
        //-  CT_CreateFile(string DeviceName)
        //-
        //--- DESCRIPTION:
        //--    Get a handle (opens file) to the HID device
        //-     returns  0 is no success - Returns 1 if success
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe int CT_CreateFile(string DeviceName)
        {

            HidHandle = CreateFile(
                DeviceName,
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                0,
                OPEN_EXISTING,
                0,
                0);
            if (HidHandle == -1)
            {
                return 0;
            }
            else
            {
                return 1;
            }

        }

        /// <summary>
        /// 打开接口
        /// </summary>
        /// <param name="DeviceName"></param>
        /// <returns></returns>
        public unsafe bool CT_CreateFile1(ref int createHandle, string DeviceName)
        {

            createHandle = CreateFile(
                DeviceName,
                0,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                0,
                OPEN_EXISTING,
                0,
                0);
            if (createHandle == -1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_CloseHandle(int hObject)
        //-
        //--- DESCRIPTION:
        //--    Closed the file and disposes of the handle
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe int CT_CloseHandle(int hObject)
        {


            int result = CloseHandle(hObject);
            // System.Windows.Forms.MessageBox.Show(result.ToString()+"  "+hObject.ToString());
            //hObject = -1;


            return result;
        }

        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_HidD_GetAttributes(int hObject))
        //-
        //--- DESCRIPTION:
        //--    Get a handle to the HID device
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe int CT_HidD_GetAttributes(int hObject)
        {
            // Create an instance of HIDD_ATTRIBUTES
            myHIDD_ATTRIBUTES = new HIDD_ATTRIBUTES();
            // Calculate its size
            myHIDD_ATTRIBUTES.Size = sizeof(HIDD_ATTRIBUTES);

            return HidD_GetAttributes(
                    hObject,
                    ref myHIDD_ATTRIBUTES);
        }

        /// <summary>
        /// 得到id号
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        public unsafe bool CT_HidD_GetAttributes1(int createHandle, ref string vID, ref string pID)
        {
            // Create an instance of HIDD_ATTRIBUTES
            myHIDD_ATTRIBUTES = new HIDD_ATTRIBUTES();
            // Calculate its size
            myHIDD_ATTRIBUTES.Size = sizeof(HIDD_ATTRIBUTES);

            int result = HidD_GetAttributes(
                    createHandle,
                    ref myHIDD_ATTRIBUTES);
            if (result != 0)
            {
                vID = myHIDD_ATTRIBUTES.VendorID.ToString("x");
                pID = myHIDD_ATTRIBUTES.ProductID.ToString("x");
                return true;
            }
            else return false;

        }
        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_HidD_GetPreparsedData(int hObject, ref int pPHIDP_PREPARSED_DATA)
        //-
        //--- DESCRIPTION:
        //--    Gets a pointer to the preparsed data buffer
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe int CT_HidD_GetPreparsedData(int hObject, ref int pPHIDP_PREPARSED_DATA)
        {
            return HidD_GetPreparsedData(
            hObject,
            ref pPHIDP_PREPARSED_DATA);
        }

        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_HidD_SetOutputReport(int HidDeviceObject, ref byte lpReportBuffer, int ReportBufferLength)
        //-
        //--- DESCRIPTION:
        //--    
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe bool CT_HidD_SetOutputReport(int HidDeviceObject, ref byte lpReportBuffer, int ReportBufferLength)
        {
            return HidD_SetOutputReport(HidDeviceObject, ref lpReportBuffer, ReportBufferLength);
        }

        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_HidP_GetCaps(int pPreparsedData)
        //-
        //--- DESCRIPTION:
        //--    Gets the capabilities report
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe int CT_HidP_GetCaps(int pPreparsedData)
        {
            myHIDP_CAPS = new HIDP_CAPS();
            return HidP_GetCaps(
             pPreparsedData,
             ref myHIDP_CAPS);
        }

        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_HidP_GetValueCaps(ref int CalsCapsLength, int pPHIDP_PREPARSED_DATA)
        //-
        //--- DESCRIPTION:
        //--    Value Capabilities
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public int CT_HidP_GetValueCaps(ref int CalsCapsLength, int pPHIDP_PREPARSED_DATA)
        {

            HIDP_REPORT_TYPE myType = 0;
            myHIDP_VALUE_CAPS = new HIDP_VALUE_CAPS[5];
            return HidP_GetValueCaps(
                myType,
                myHIDP_VALUE_CAPS,
                ref CalsCapsLength,
                pPHIDP_PREPARSED_DATA);

        }

        //---#+************************************************************************
        //---NOTATION:
        //-  byte[] CT_ReadFile(int InputReportByteLength)
        //-
        //--- DESCRIPTION:
        //--    read Port
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe byte[] CT_ReadFile(int InputReportByteLength)
        {
            int BytesRead = 0;
            byte[] BufBytes = new byte[InputReportByteLength];
            if (ReadFile(HidHandle, ref BufBytes, InputReportByteLength, ref BytesRead, null))
            {
                byte[] OutBytes = new byte[BytesRead];

                Array.Copy(BufBytes, OutBytes, BytesRead);
                return OutBytes;
            }
            else
            {
                return null;
            }
        }
        [DllImport("hid.dll")]
        static public extern bool HidD_GetInputReport(int HidDeviceObject, ref byte lpReportBuffer, int ReportBufferLength);


        /// <summary>
        /// 读文件操作
        /// </summary>
        /// <param name="InputReportByteLength"></param>
        /// <returns></returns>
        public unsafe bool ReadHidFile(int readHandle, ref byte readValue, ref int readLength)
        {

            //  SECURITY_ATTRIBUTES Security=new SECURITY_ATTRIBUTES();
            //  Security.lpSecurityDescriptor=0;
            //  Security.bInheritHandle=System.Convert.ToInt32(true);
            //  Security.nLength=Marshal.SizeOf(Security);

            //  OVERLAPPED HIDOverlapped = new OVERLAPPED();
            //  HIDOverlapped.Offset = 0;
            //  HIDOverlapped.OffsetHigh = 0;
            //  HIDOverlapped.hEvent = CreateEvent(ref Security, Convert.ToInt32(false), Convert.ToInt32(true), "");
            ////  System.Windows.Forms.MessageBox.Show(readHandle.ToString());

            //bool result2 =HidD_SetOutputReport(readHandle, ref readValue[0], 64);
            //System.Windows.Forms.MessageBox.Show(readValue[0].ToString());
            // readValue = 1;

            int result = ReadFile(readHandle, ref readValue, 1024, ref readLength, 0);
            //  System.Windows.Forms.MessageBox.Show(result2.ToString());
            // System.Windows.Forms.MessageBox.Show(System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString());
            // int a=0;
            //  System.Windows.Forms.MessageBox.Show(result.ToString());
            // bool a = ReadFile(readHandle,ref readValue, 64, ref readLength, null);
            //

            //  int  result1 = WaitForSingleObject(HIDOverlapped.hEvent, 6000);
            if (result != 0) return true;
            else
                return false;
            // System.Windows.Forms.MessageBox.Show(a.ToString());
            //switch (result1)
            //{ 
            //    case WAIT_OBJECT_0:
            //       // System.Windows.Forms.MessageBox.Show(readLength.ToString());
            //        return true;
            //    case WAIT_TIMEOUT:
            //        return false;
            //    default: return false;
            //}
        }


        /// <summary>
        /// 得到输入ID
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="outputValueCaps"></param>
        /// <returns></returns>
        public unsafe bool GetDeviceCapabilities(int handle, ref string outputValueCaps, ref string outputLength)
        {
            HIDP_CAPS capabilities = new HIDP_CAPS();
            int PreparsedDataPointer = 0;
            int Result;
            try
            {
                Result = HidD_GetPreparsedData(handle, ref PreparsedDataPointer);
                Result = HidP_GetCaps(PreparsedDataPointer, ref capabilities);
                if (Result != 0)
                {
                    outputValueCaps = capabilities.NumberOutputValueCaps.ToString();
                    outputLength = capabilities.OutputReportByteLength.ToString();
                    return true;
                }
                else return false;

            }
            catch
            {
                return false;
            }
        }
        //---#+************************************************************************
        //---NOTATION:
        //-  CT_WriteFile(int hFile, ref byte lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, int lpOverlapped)
        //-
        //--- DESCRIPTION:
        //--    write Port
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public unsafe int CT_WriteFile(int hFile, ref byte lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, int lpOverlapped)
        {
            return WriteFile(hFile, ref lpBuffer, nNumberOfBytesToWrite, ref lpNumberOfBytesWritten, lpOverlapped);
        }

        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_SetupDiDestroyDeviceInfoList()
        //-
        //--- DESCRIPTION:
        //--    DestroyDeviceInfoList
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public int CT_SetupDiDestroyDeviceInfoList()
        {
            return SetupDiDestroyDeviceInfoList(hDevInfo);

        }

        //---#+************************************************************************
        //---NOTATION:
        //-  int CT_HidD_FreePreparsedData(int pPHIDP_PREPARSED_DATA)
        //-
        //--- DESCRIPTION:
        //--    FreePreparsedData
        //
        //                                                              Autor:      F.L.
        //-*************************************************************************+#*
        public int CT_HidD_FreePreparsedData(int pPHIDP_PREPARSED_DATA)
        {
            return SetupDiDestroyDeviceInfoList(pPHIDP_PREPARSED_DATA);
        }

        internal HidApiDeclarations HidApiDeclarations
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }




    }
}
