using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Engraving.MarkAPI;

namespace Engraving
{
    internal class Program
    {
        static HandleMsg handleMsg;
        private String m_strDevId;          //设备id
        private String m_strFileId;        //选中的文件

        private static MarkAPI MarkAPI; //加载MarkSDK.dll动态库

        private static MarkAPI Calib;   //加载Caib.dll动态库



        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal class DEV_BROADCAST_DEVICEINTERFACE
        {
            public int dbcc_size;
            public uint dbcc_devicetype;
            public int dbcc_reserved;
            public Guid dbcc_classguid;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
            public byte[] dbcc_name;        ////设备名称
            // public IntPtr dbcc_name;
        }


        public const int WM_DEVICECHANGE = 0x0219;
        public const int WM_DEVICECHANGE_UDP = 0x07E9;
        public const int DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;  // device interface class
        public const int DBT_DEVTYP_HANDLE = 6;


        public const int DEVICE_NOTIFY_WINDOW_HANDLE = (0x00000000);

        static Guid CyGuid = new Guid("{0xae18aa60, 0x7f6a, 0x11d4, {0x97, 0xdd, 0x0, 0x1, 0x2, 0x29, 0xb9, 0x59}}");


        // 声明Windows API函数
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, DEV_BROADCAST_DEVICEINTERFACE NotificationFilter, UInt32 Flags);

        static void Main(string[] args)
        {
            

            Task.Factory.StartNew(() =>
            {
                handleMsg = new HandleMsg();
                Application.Run(handleMsg);
            });

            while (handleMsg == null)
            {
                Thread.Sleep(50);
            }
            initialize();
            Listener listener = new Listener();
            listener.HttpServer();
        }

        /// <summary>
        /// 系统初始化
        /// </summary>
        /// 
        public static void initialize()
        {
            Console.WriteLine(HandleMsg.Message);
        }

        /// <summary>
        /// 允许一个窗口或者服务接收所有硬件的通知
        /// 但是好像没啥用
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool AllowNotifications(IntPtr callback)
        {
            try
            {
                DEV_BROADCAST_DEVICEINTERFACE dFilter = new DEV_BROADCAST_DEVICEINTERFACE();
                dFilter.dbcc_size = Marshal.SizeOf(dFilter);
                dFilter.dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE;
                dFilter.dbcc_classguid = CyGuid;

                RegisterDeviceNotification(callback, dFilter, DEVICE_NOTIFY_WINDOW_HANDLE);
                return true;
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return false;
            }
        }

    }
}
