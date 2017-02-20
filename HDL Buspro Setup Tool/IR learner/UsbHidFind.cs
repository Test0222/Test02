using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
   public class UsbHidFind
    {
       private USBSharp myUsb = new USBSharp();
       /// <summary>
       /// 寻找hid设备
       /// </summary>
       /// <param name="hidGuid"></param>
       /// <param name="vendorID"></param>
       /// <param name="productID"></param>
       /// <param name="devicePath"></param>
       /// <param name="deviceCount"></param>
       /// <returns></returns>
       public bool FindHidDevice(ref string hidGuid,ref string[] vendorID, ref string[] productID, ref string[] devicePath,ref string[] outputID,ref string[] outputLength, ref int deviceCount)
       {
           try
           {
               bool DeviceFound = false;
               deviceCount = 0;
               hidGuid = myUsb.CT_HidGuid1();
               myUsb.CT_SetupDiGetClassDevs();
               int result = -1;
               int resultb = -1;
               int requiredSize = 0;
               int size = 0;
               while (result != 0)
               {
                   result = myUsb.CT_SetupDiEnumDeviceInterfaces(deviceCount);
                   resultb = myUsb.CT_SetupDiGetDeviceInterfaceDetail(ref requiredSize, 0);
                   size = requiredSize;
                   resultb = myUsb.CT_SetupDiGetDeviceInterfaceDetailx(ref devicePath[deviceCount], ref requiredSize, size);
                   deviceCount++;
                   DeviceFound = true;
               }
               int nowDeviceCount = 0;
               if (DeviceFound)
               {
                   for (int i = 0; i <= deviceCount; i++)
                   {
                       int createHandle = 1;
                       if (myUsb.CT_CreateFile1(ref createHandle, devicePath[i]))//成功
                       {
                           if (myUsb.CT_HidD_GetAttributes1(createHandle, ref vendorID[i], ref productID[i]))
                           {
                               if (myUsb.GetDeviceCapabilities(createHandle, ref outputID[i], ref outputLength[i]))
                                   nowDeviceCount++;
                           }
                           myUsb.CT_CloseHandle(createHandle);
                       }
                   }
                   if (nowDeviceCount == 0) return false;
                   else
                   {
                       deviceCount = nowDeviceCount - 1;
                       return true;
                   }
               }
               else
               {
                   return false;
               }
           }
           catch
           {
               return false;
           }
       }
       /// <summary>
       /// 写HID文件
       /// </summary>
       /// <param name="devicePath"></param>
       /// <param name="writeValue"></param>
       /// <returns></returns>
       public bool WriteHidFile(string devicePath,int writeLength,int writeID,byte[] writeValue,ref byte[] writeData)
       {
           try
           {
               int writeHandle = myUsb.CreateWriteFile(devicePath);
               if (writeHandle != -1)//打开句柄成功
               {
                   byte[] writeHidVlue = new byte[writeLength + 1];
                   writeHidVlue[0] = Convert.ToByte(writeID);
                   for (int i = 1; i < writeLength + 1; i++)
                   {
                       writeHidVlue[i] = writeValue[i - 1];
                   }
                   
                   //写入信息
                   int writeNumber = 0;
                   int result = myUsb.CT_WriteFile(writeHandle, ref writeHidVlue[0], writeLength, ref writeNumber, 0);
                   writeData = writeHidVlue;//显示已经写入的信息
                   myUsb.CT_CloseHandle(writeHandle);
                   if (result != 0) return true;
                   else return false;

               }
               else
               {
                   myUsb.CT_CloseHandle(writeHandle);
                   return false;
               }
           }
           catch
           {
               return false;
           }
       }


       /// <summary>
       /// 写HID文件
       /// </summary>
       /// <param name="devicePath"></param>
       /// <param name="writeValue"></param>
       /// <returns></returns>
       public bool WriteHidFile(string devicePath, int writeLength, byte[] writeValue, ref byte[] writeData)
       {
           try
           {
               int writeHandle = myUsb.CreateWriteFile(devicePath);
               if (writeHandle != -1)//打开句柄成功
               {
                   byte[] writeHidVlue = new byte[writeLength];
                 //  writeHidVlue[0] = Convert.ToByte(writeID);
                   if (writeValue.Length >= writeLength)
                   {
                       for (int i = 0; i < writeLength; i++)
                       {
                           writeHidVlue[i] = writeValue[i];
                       }
                   }
                   else
                   {
                       for (int i = 0; i < writeValue.Length; i++)
                       {
                           writeHidVlue[i] = writeValue[i];
                       }
                       for (int i = writeValue.Length; i < writeLength; i++)
                       {
                           writeHidVlue[i] = 0;
                       }
                   }
                   //写入信息
                   int writeNumber = 0;
                   int result = myUsb.CT_WriteFile(writeHandle, ref writeHidVlue[0], writeLength, ref writeNumber, 0);
                   //System.Windows.Forms.MessageBox.Show(writeNumber.ToString()+" "+result.ToString());
                   //System.Threading.Thread.Sleep(100);
                   CsConst.isWriteDataToUSB = false;
                   writeData = writeHidVlue;//显示已经写入的信息
                   myUsb.CT_CloseHandle(writeHandle);
                   if (result != 0) return true;
                   else return false;

               }
               else
               {
                   myUsb.CT_CloseHandle(writeHandle);
                   return false;
               }
           }
           catch
           {
               return false;
           }
       }
       int readHandle;
       /// <summary>
       /// 读hid文件
       /// </summary>
       /// <param name="devicePath"></param>
       /// <param name="readValue"></param>
       /// <param name="readLength"></param>
       /// <returns></returns>
       public bool ReadHidFile(string devicePath,ref byte[] readValue,ref int readLength)
       {
           try
           {
              // System.Threading.Thread.Sleep(100);
               readHandle = myUsb.CreateReadFile(devicePath);
               if (readHandle != -1)
               {
                   bool result = myUsb.ReadHidFile(readHandle, ref readValue[0], ref readLength);
                   myUsb.CT_CloseHandle(readHandle);
                   if (result)
                   {
                       return true;
                   }
                   else
                   {
                       return false;
                   }
               }
               else
               {
                   myUsb.CT_CloseHandle(readHandle);
                   return false;
               }
           }
           catch
           {
               return false;
           }
       }
    }
}
