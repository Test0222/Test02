using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    internal class UDPReceive : IDisposable
    {
        [DllImport("ntdll.dll")]
        public static extern int RtlCompareMemory(IntPtr Destination, IntPtr Source, int Length);

        public void Dispose()
        {
            try
            {
                receiveQueue.Clear();
                receiveQueueForCurtain.Clear();
                receiveQueueForSingnal.Clear();
                receiveQueueForUpgrade.Clear();
                receiveQueueForAudio.Clear();
            }
            catch
            {

            }
        }
        public static Queue<Byte[]> receiveQueueForUpgrade = new Queue<byte[]>();
        public static Queue<byte[]> receiveQueue = new Queue<byte[]>();
        public static Queue<byte[]> receiveQueueForCurtain = new Queue<byte[]>();
        public static Queue<byte[]> receiveQueueForSingnal = new Queue<byte[]>();
        public static Queue<byte[]> receiveQueueForAudio = new Queue<byte[]>();
        public static Queue<byte[]> receiveBuffer = new Queue<byte[]>();

        public static void OnReceiveForBuffer(byte[] receiveData)
        {
            receiveBuffer.Enqueue(receiveData);
        }

        public static void OnReceive(byte[] receiveData)
        {
            receiveQueue.Enqueue(receiveData);
        }

        public static void OnReceiveForSingnal(byte[] receiveData)
        {
            receiveQueueForSingnal.Enqueue(receiveData);
        }

        public static void OnReceiveForCurtain(byte[] receiveData)
        {
            receiveQueueForCurtain.Enqueue(receiveData);
        }

        public static void OnReceiveForUpgrade(byte[] receiveData)
        {
            if (receiveQueueForUpgrade.Count <= 0)
            {
                receiveQueueForUpgrade.Enqueue(receiveData);
            }
            else 
            {
                Boolean IsHasSameData = false;

                //foreach (Byte[] Tmp in receiveQueueForUpgrade)
                //{
                //    int id = RtlCompareMemory(Marshal.UnsafeAddrOfPinnedArrayElement(Tmp, 0),
                //        Marshal.UnsafeAddrOfPinnedArrayElement(receiveData, 0), sizeof(int) * Tmp.Length);
                //    if (id >= Tmp.Length)
                //    {
                //        IsHasSameData = true;
                //        break;
                //    }
                //}
               if (IsHasSameData == false)  receiveQueueForUpgrade.Enqueue(receiveData);
            }
        }

        public static void OnReceiveForAudio(byte[] receiveData)
        {
            receiveQueueForAudio.Enqueue(receiveData);
        }

        public static void ClearQueueData()
        {
            receiveQueue.Clear();
        }

        public static void ClearQueueDataForCurtain()
        {
            receiveQueueForCurtain.Clear();
        }

        public static void ClearQueueDataForSingnal()
        {
            receiveQueueForSingnal.Clear();
        }

        public static void ClearQueueDataForUpgrade()
        {
            receiveQueueForUpgrade.Clear();
        }

        public static void ClearQueueDataForAudio()
        {
            receiveQueueForAudio.Clear();
        }
    }

}