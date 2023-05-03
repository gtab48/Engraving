using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Engraving.MarkAPI;
using static Engraving.Program;

namespace Engraving
{
    public partial class HandleMsg : Form
    {
        //当前设备ID
        public String DeviceID;

        public static string[] ID;

        //消息
        public static string Message;

        //获取窗口句柄
        public static IntPtr intPtr;

        //加载MarkSDK.dll动态库
        private static MarkAPI MarkAPI = new MarkAPI("MarkSDK.dll");
        //加载Caib.dll动态库
        private static MarkAPI Calib = new MarkAPI("Calib.dall");

        static int DBT_DEVICEARRIVAL = 0x8000;     //设备插入
        static int DBT_DEVICEREMOVECOMPLETE = 0x8004;  //设备移除

        //是否停止
        private bool stop;

        public const int DEVICE_NOTIFY_WINDOW_HANDLE = (0x00000000);
        public const int WM_DEVICECHANGE = 0x0219;
        public const int WM_DEVICECHANGE_UDP = 0x07E9;
        public const int DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;  // device interface class
        public const int DBT_DEVTYP_HANDLE = 6;

        static Guid CyGuid = new Guid("{0xae18aa60, 0x7f6a, 0x11d4, {0x97, 0xdd, 0x0, 0x1, 0x2, 0x29, 0xb9, 0x59}}");


        struct DEV_BROADCAST_HDR
        {
            public Int32 dbch_size;//dbch_size表示结构体实例的字节数
            public Int32 dbch_devicetype; //dbch_devicetype字段值等于DBT_DEVTYP_VOLUME时，表示当前设备是逻辑驱动器
            public IntPtr dbch_reserved;
            public IntPtr dbch_handle;
        }

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
        //设备列表
        public static Dictionary<string, string> map = new Dictionary<string, string>();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, DEV_BROADCAST_DEVICEINTERFACE NotificationFilter, UInt32 Flags);
        public HandleMsg()
        {
            InitializeComponent();
            stop = true;
        }

        private void HandleMsg_Load(object sender, EventArgs e)
        {
            //隐藏窗口
            this.BeginInvoke(new Action(() =>
            {
                this.Hide();
                this.Opacity = 1;
            }));

            intPtr = this.Handle;
            BSL_UDPInit func = (BSL_UDPInit)MarkAPI.Invoke("UDPInit", typeof(BSL_UDPInit));
            if (func != null)
            {
                BslErrCode iRes = func(this.Handle, WM_DEVICECHANGE_UDP);
                if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                {
                }
            }

            PathDataShape shape = new PathDataShape();

            int icnt = Marshal.SizeOf(shape);

            AllowNotifications(this.Handle, true);
        }

        /// <summary>
        /// 允许一个窗口或者服务接收所有硬件的通知
        /// 注:目前还没有找到一个比较好的方法来处理如果通知服务。
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="UseWindowHandle"></param>
        /// <returns></returns>
        public bool AllowNotifications(IntPtr callback, bool UseWindowHandle)
        {
            try
            {
                DEV_BROADCAST_DEVICEINTERFACE dFilter = new DEV_BROADCAST_DEVICEINTERFACE();
                dFilter.dbcc_size = Marshal.SizeOf(dFilter);
                dFilter.dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE;
                dFilter.dbcc_classguid = CyGuid;

                IntPtr iHandle = RegisterDeviceNotification(Handle, dFilter, DEVICE_NOTIFY_WINDOW_HANDLE);
                return true;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return false;
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                int nEventType = m.WParam.ToInt32();
                if (nEventType >= 0x8000)
                {
                    DEV_BROADCAST_HDR lpdb = new DEV_BROADCAST_HDR();
                    lpdb = (DEV_BROADCAST_HDR)m.GetLParam(lpdb.GetType());
                    switch (lpdb.dbch_devicetype)
                    {
                        case DBT_DEVTYP_DEVICEINTERFACE:
                            {
                                DEV_BROADCAST_DEVICEINTERFACE pDevInf = (DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_DEVICEINTERFACE));
                                //设备名称
                                String strDevPath = System.Text.Encoding.Unicode.GetString(pDevInf.dbcc_name).TrimEnd('\0');
                                int iPos = strDevPath.IndexOf('\0');
                                if (iPos > 0)
                                {//去掉多余的\0
                                    strDevPath = strDevPath.Substring(0, iPos);
                                }
                                if (nEventType == DBT_DEVICEARRIVAL)  //DBT_DEVICEARRIVAL 有设备插入
                                {//插入新设备
                                    object[] param = new object[3];
                                    param[0] = MarkAPI;
                                    param[1] = strDevPath;		//当前变化的设备路径
                                    param[2] = true;

                                    ThreadReInitDevices(param);
                                    //   Thread threadC = new Thread(ThreadReInitDevices);
                                    //   threadC.Start(param);
                                }
                                else if (nEventType == DBT_DEVICEREMOVECOMPLETE)  //DBT_DEVICEREMOVECOMPLETE 有设备拔出
                                {//拔掉设备
                                    object[] param = new object[3];
                                    param[0] = MarkAPI;
                                    param[1] = strDevPath;		//当前变化的设备路径
                                    param[2] = false;

                                    ThreadReInitDevices(param);
                                    //    Thread threadC = new Thread(ThreadReInitDevices);
                                    //    threadC.Start(param);
                                }
                                break;
                            }
                        default:
                            break;
                    }//switch
                }
            }
            else if (m.Msg == WM_DEVICECHANGE_UDP)
            {
                BSL_UDPDeviceChanged func = (BSL_UDPDeviceChanged)MarkAPI.Invoke("OnUDPDeviceChanged", typeof(BSL_UDPDeviceChanged));
                if (func != null)
                {
                    BslErrCode iRes = func(m.WParam, m.LParam);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        bool bAdd = false;
                        if (m.WParam.ToInt32() == 3)
                        {
                            //这里必须等待标刻线程退出后
                            while (!this.stop) ;

                            //刷新设备列表
                            this.Invoke((EventHandler)delegate { GetFreshDevlist(); });
                        }
                        else
                        {
                            //刷新设备列表
                            this.Invoke((EventHandler)delegate { GetFreshDevlist(); });
                        }

                    }
                }
            }

            //调用基类的同名方法
            base.WndProc(ref m);
        }

        /// <summary>
        /// 刷新或获取设备ID
        /// </summary>
        /// <returns></returns>
        public void GetFreshDevlist()
        {
            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_GetAllDevices func = (BSL_GetAllDevices)MarkAPI.Invoke("GetAllDevices2", typeof(BSL_GetAllDevices));
                if (func != null)
                {
                    int iDevCount = 0;
                    STU_DEVID[] vDevID = new STU_DEVID[10];
                    BslErrCode iRes = func(vDevID, ref iDevCount);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        int iCount = iDevCount;
                        for (int i = 0; i < iCount; i++)
                        {
                            string str = System.Text.Encoding.Default.GetString(vDevID[i].wszDevName).TrimEnd('\0');
                            if (DeviceID == null)
                            {
                                DeviceID = str;
                            }
                            map.Add("设备名称" + i, str);
                            ID = new string[map.Count];
                            map.Values.CopyTo(ID, 0);
                        }
                    }
                    else
                    {
                        string str;
                        str = "operating fail errorcode = " + iRes;
                        MessageBox.Show(str);
                    }
                    vDevID = null;
                    GC.Collect();
                }
            }
        }

        void ThreadReInitDevices(object obj)
        {
            object[] param = (object[])obj;
            bool bAdd = (bool)param[2];
            if (!bAdd)
            {//拔出设备
                //停止标刻
                this.Invoke((EventHandler)delegate { HttpServer.DeviceStop(DeviceID); });
                //这里必须等待标刻线程退出后
                while (!this.stop) ;
            }
            else
            {//插入设备
            }

            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_ReInitDevices func = (BSL_ReInitDevices)MarkAPI.Invoke("ReInitDevices", typeof(BSL_ReInitDevices));
                if (func != null)
                {
                    string strDevid = (string)param[1];

                    BslErrCode nCount = func(strDevid, bAdd);
                }
            }
            //刷新设备列表
            this.Invoke((EventHandler)delegate { GetFreshDevlist(); });
        }
    }
}
