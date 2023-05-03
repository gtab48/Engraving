using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Engraving.MarkAPI;
using System.Diagnostics.SymbolStore;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Engraving.HandleMsg;
using System.Threading;
using System.Windows.Forms;
using System.Reflection.Emit;
using System.Drawing;
using System.Collections;

namespace Engraving
{
    public class HttpServer
    {

        private static MarkAPI MarkAPI = new MarkAPI("MarkSDK.dll"); //加载MarkSDK.dll动态库

        private static MarkAPI Calib = new MarkAPI("Calib.dall");   //加载Caib.dll动态库

        private static string engravingMessage; //标刻信息

        private static int Threads = 0;

        private static string message = null;

        public static bool stop = true;

        private static List<string> m_ListParNames; //参数列表

        /// <summary>
        /// 返回Message信息
        /// </summary>
        /// <returns></returns>
        public static string Message()
        {
            return message;
        }
        /// <summary>
        /// 获取设备ID
        /// </summary>
        /// <returns></returns>
        public static string GetFreshDevlist()
        {
            string DevID = JsonConvert.SerializeObject(map);

            return DevID;

        }

        ///<summary>
        ///关联设备参数
        ///</summary>
        ///<param name="DevId">设备Id</param>
        ///<param name="ParName">关联设备Id</param>
        public static string DeviceParameters(string DevId, string ParName)
        {
            if (DevId == null || ParName == null)
            {
                return "参数错误";
            }
            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_GetAllPara func = (BSL_GetAllPara)MarkAPI.Invoke("GetAllDevices2", typeof(BSL_GetAllPara));
                if (func != null)
                {
                    STU_PARA[] vParName = new STU_PARA[100];
                    int iParCount = 0;
                    BslErrCode iRes = func(vParName, ref iParCount);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        List<string> ListParNames = new List<string>();
                        for (int i = 0; i < iParCount; i++)
                        {
                            string str = System.Text.Encoding.Default.GetString(vParName[i].wszParaName).TrimEnd('\0');
                            ListParNames.Add(str);
                        }
                        BSL_AssocDevPara funcl = (BSL_AssocDevPara)MarkAPI.Invoke("AssocDevPara", typeof(BSL_AssocDevPara));
                        if (funcl != null)
                        {
                            iRes = funcl(DevId, ParName);
                            message = iRes.ToString();
                        }
                        else
                        {
                            message = "operating fail errorcode = " + iRes;
                        }
                    }
                }
                else
                {
                    message = "获取参数失败";
                }
            }
            return message;
            GC.Collect();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static string DeviceStop(string DeviceID)
        {
            string stopType = "操作失败";
            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_StopMark func = (BSL_StopMark)MarkAPI.Invoke("StopMark", typeof(BSL_StopMark));
                if (func != null)
                {
                    BslErrCode iRes = func(DeviceID);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        stop = true;
                        return stopType = "当前设备已停止";
                    }
                }
            }
            return stopType;
        }

        ///<summary>
        ///单卡标刻
        /// </summary>
        /// <param name="DeviceID">设备ID</param>
        /// <param name="dAngle">旋转角度</param>
        /// <param name="dCenterX">动态偏移X</param>
        /// <param name="dCenterY">动态偏移Y</param>
        /// <param name="dOffsetX">旋转中心X</param>
        /// <param name="dOffsetY">旋转中心Y</param>
        public static string Engraving(string DeviceID, double dAngle, double dOffsetX, double dOffsetY, double dCenterX, double dCenterY)
        {
            if (DeviceID != null)
            {
                if (MarkAPI.hLib != IntPtr.Zero)
                {
                    if (!stop)
                    {
                        DeviceStop(DeviceID);
                        Thread.Sleep(500);
                    }
                    stop = false;

                    BSL_SetOffsetValues func = (BSL_SetOffsetValues)MarkAPI.Invoke("SetOffsetValues", typeof(BSL_SetOffsetValues));
                    if (func != null)
                    {
                        func(dAngle, dOffsetX, dOffsetY, dCenterX, dCenterY);
                    }
                    Threads = 1;

                    object[] param = new object[2];
                    param[0] = MarkAPI;
                    param[1] = DeviceID;

                    Thread thread = new Thread(ThreadMarkCard);
                    thread.Start(param);

                    message = "标刻中";
                }
            }
            else
            {
                message = "请输入设备ID";
            }
            return message;
        }
        /// <summary>
        /// 多卡标刻
        /// </summary>
        /// <param name="dAngle">旋转角度</param>
        /// <param name="dCenterX">动态偏移X</param>
        /// <param name="dCenterY">动态偏移Y</param>
        /// <param name="dOffsetX">旋转中心X</param>
        /// <param name="dOffsetY">旋转中心Y</param>
        /// <returns></returns>
        public static string EngravingAll(double dAngle, double dOffsetX, double dOffsetY, double dCenterX, double dCenterY)
        {
            if (MarkAPI.hLib != IntPtr.Zero)
            {
                stop = false;

                BSL_SetOffsetValues func = (BSL_SetOffsetValues)MarkAPI.Invoke("SetOffsetValues", typeof(BSL_SetOffsetValues));
                if (func != null)
                {
                    func(dAngle, dOffsetX, dOffsetY, dCenterX, dCenterY);
                }
                Threads = 1;
                Thread thread = new Thread(ThreadMarkCard);
                for (int i = 0; i < ID.Length; i++)
                {
                    object[] param = new object[2];
                    param[0] = MarkAPI;
                    param[1] = ID[i];

                    
                    thread.Start(param);
                }

                message = "标刻中";
            }
            return message;
        }

        static void ThreadMarkCard(object obj)
        {
            object[] param = (object[])obj;
            MarkAPI hMarkDll = (MarkAPI)param[0];
            String strDevId = param[1].ToString();
            message = null;
            DateTime beforDT = System.DateTime.Now;
            BSL_MarkByDeviceId func = (BSL_MarkByDeviceId)MarkAPI.Invoke("MarkByDeviceId", typeof(BSL_MarkByDeviceId));

            if (MarkAPI.hLib != IntPtr.Zero)
            {
                if (func != null)
                {
                    StringBuilder ssID = new StringBuilder(strDevId);
                    BslErrCode iRes = func(strDevId);

                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        DateTime afterDT = System.DateTime.Now;
                        TimeSpan ts = afterDT.Subtract(beforDT);
                        message = "标刻完成总共花费" + ts.TotalMilliseconds + "ms";
                    }
                    else
                    {
                        message = "标刻失败";
                    }
                }
                Threads--;
                if (Threads == 0)
                {
                    stop = true;
                }
            }
        }

        /// <summary>
        /// 红光启动
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        public static string RedLihtDisplay(string DeviceID)
        {
            string message = null;
            if (!stop) 
            {
                message = "请先停止设备";
                return message;
            }
            if (MarkAPI.hLib != IntPtr.Zero) 
            {
                stop = false;

                object[] param  = new object[2];
                param[0] = MarkAPI;
                param[1] = DeviceID;
                Thread thread = new Thread(ThreadRedLight);
                thread.Start(param);
                message = "红光选定启动";
            }
            return message;
        }

        //红光显示选定 线程函数
        static void ThreadRedLight(object DevId)
        {
            object[] param = (object[])DevId;
            MarkAPI hMarkDll = (MarkAPI)param[0];
            String strDevId = param[1].ToString();
            BSL_RedLightMark func = (BSL_RedLightMark)MarkAPI.Invoke("RedLightMark", typeof(BSL_RedLightMark));
            if (func != null)
            {
                BslErrCode iRes = func(strDevId, true);
                if (iRes != BslErrCode.BSL_ERR_SUCCESS)
                {
                    message = "Error";
                }
            }
        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="strFile">文件路径</param>
        /// <returns></returns>
        public static string LoadFile(string strFile)
        {
            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_LoadDataFile func = (BSL_LoadDataFile)MarkAPI.Invoke("LoadDataFile", typeof(BSL_LoadDataFile));
                if (func != null)
                {
                    BslErrCode iRes = func(strFile);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        message = "文件加载成功";
                        //获取文件中所有图形信息
                        ShowShapeList(strFile);
                    }
                    else
                    {
                        message = "文件加载失败";
                        return message;
                    }
                }
            }
            return message;
        }

        static void ShowShapeList(string strFile)
        {

            Dictionary<string, string> Show = new Dictionary<string,string>();
            if (MarkAPI.hLib != IntPtr.Zero) 
            {
                int iCount = 0;
                BSL_GetEntityCount funcGetShapeCount = (BSL_GetEntityCount)MarkAPI.Invoke("GetEntityCount", typeof(BSL_GetEntityCount));
                if (funcGetShapeCount != null)
                {
                    String str = "图元列表";
                    iCount = (int)funcGetShapeCount(strFile);
                    if (iCount == (int)BslErrCode.BSL_ERR_WRONGPARAM)
                    {
                        message = "获取图元数目失败";
                    }
                    else
                    {
                        str = "共" + iCount + "个图元";
                    }
                    Show.Add("图元数量", str);
                }
                if (iCount <= 0)
                {
                    message = "目标文件没有图元信息";
                    return ;
                }
                BSL_GetShapesInFile2 funcGetShapes = (BSL_GetShapesInFile2)MarkAPI.Invoke("GetShapesInFile2", typeof(BSL_GetShapesInFile2));
                if (funcGetShapes != null)
                {
                    ShapeInfo2[] vShapes = new ShapeInfo2[iCount];

                    BslErrCode iRes = funcGetShapes(strFile, vShapes, iCount);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        // 获取文件中所有图形信息
                        for (int i = 0; i < iCount; i++)
                        {
                            string str = System.Text.Encoding.Default.GetString(vShapes[i].wszShapeName).TrimEnd('\0');
                            Show.Add("图形信息"+i, str);
                        }                            
                    }
                }
                
                GC.Collect();
            }
            message = JsonConvert.SerializeObject(Show);
        }
        /// <summary>
        /// 关闭文件
        /// </summary>
        /// <param name="strFile">文件路径</param>
        /// <returns></returns>
        public static string UnLoadFile(string strFile)
        {
            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_UnLoadDataFile func = (BSL_UnLoadDataFile)MarkAPI.Invoke("UnloadDataFile", typeof(BSL_UnLoadDataFile));
                if (func != null)
                {
                    BslErrCode iRes = func(strFile);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        message = "关闭成功";
                    }
                }
            }
            return message;
        }


        /// <summary>
        /// 标刻选定的图元
        /// </summary>
        /// <param name="DeviceID">设备ID</param>
        /// <param name="strFile">文件路径</param>
        /// <param name="shpID">图元编号</param>
        /// <param name="dAngle">旋转角度</param>
        /// <param name="dCenterX">动态偏移X</param>
        /// <param name="dCenterY">动态偏移Y</param>
        /// <param name="dOffsetX">旋转中心X</param>
        /// <param name="dOffsetY">旋转中心Y</param>
        /// <returns></returns>
        public static string MarkEntity(string DeviceID, string strFile, string shpID, double dAngle, double dOffsetX, double dOffsetY, double dCenterX, double dCenterY)
        {
            if (MarkAPI.hLib != IntPtr.Zero)
            {
                if (!stop)
                {
                    DeviceStop(DeviceID);
                    Thread.Sleep(500);
                }
                stop = false;

                BSL_SetOffsetValues func = (BSL_SetOffsetValues)MarkAPI.Invoke("SetOffsetValues", typeof(BSL_SetOffsetValues));
                if (func != null) 
                {
                    func(dAngle, dOffsetX, dOffsetY, dCenterX, dCenterY);
                }

                object[] param = new object[4];
                param[0] = MarkAPI;
                param[1] = DeviceID;
                param[2] = strFile;
                param[3] = shpID;
                Thread threadC = new Thread(ThreadMarkShape);
                threadC.Start(param);
                message = "标刻中";
                
            }
            return message;
        }

        static void ThreadMarkShape(object obj)
        {
            object[] param = (object[])obj;
            MarkAPI hMarkDll = (MarkAPI)param[0];
            String strDevId = param[1].ToString();
            String strFileId = param[2].ToString();
            String strShpId = param[3].ToString();

            DateTime beforDT = System.DateTime.Now;
            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_MarkEntity func = (BSL_MarkEntity)MarkAPI.Invoke("MarkEntity", typeof(BSL_MarkEntity));
                if (func != null)
                {
                    BslErrCode iRes = func(strDevId, strFileId, strShpId);

                    DateTime afterDT = System.DateTime.Now;
                    TimeSpan ts = afterDT.Subtract(beforDT);


                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        message = "标刻完成总共花费" + ts.TotalMilliseconds + "ms";
                    }
                    else
                    {
                        message = "标刻失败";
                    }
                }
            }
        }

        /// <summary>
        /// 标刻一段线段
        /// </summary>
        /// <param name="DeviceID">设备ID</param>
        /// <returns></returns>
        public static string MarkLine(string DeviceID)
        {
            List<POINTF> lstPoints = new List<POINTF>();
            List<int> lstPtCount = new List<int>();

            int row = 10;
            int col = 10;
            for (int i = 0; i < row; i++)
            {
                for (int ii = 0; ii < col; ii++)
                {
                    POINTF tempPoint;
                    tempPoint.x = i + (float)0.5;
                    tempPoint.y = ii + (float)0.5;
                    lstPoints.Add(tempPoint);
                }
                lstPtCount.Add(col);
            }
            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_MarkLines func = (BSL_MarkLines)MarkAPI.Invoke("MarkLines2", typeof(BSL_MarkLines));
                if (func != null)
                {
                    BslErrCode iRes = func(DeviceID, lstPoints.ToArray(), row, lstPtCount.ToArray(), 0);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        message ="标刻完成";
                    }
                    else
                    {
                        message = "标刻失败";
                    }
                }
            }
            return message;
        }
        /// <summary>
        /// 标刻一组线段
        /// </summary>
        /// <param name="DeviceID">设备ID</param>
        /// <returns></returns>
        public static string MarkLines(string DeviceID)
        {
            List<int> lstPtCount = new List<int>();
            List<POINTF> vPoints = new List<POINTF>();

            POINTF tempPoint;
            tempPoint.x = 0;
            tempPoint.y = 0;
            vPoints.Add(tempPoint);

            tempPoint.x = 0;
            tempPoint.y = 20;
            vPoints.Add(tempPoint);

            lstPtCount.Add(2);

            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_MarkLines func = (BSL_MarkLines)MarkAPI.Invoke("MarkLines2", typeof(BSL_MarkLines));
                if (func != null)
                {
                    BslErrCode iRes = func(DeviceID, vPoints.ToArray(), 1, lstPtCount.ToArray(), 0);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        message = "标刻完成";
                    }
                    else
                    {
                        message = "标刻失败";
                    }
                }
            }
            return message;
        }
        /// <summary>
        /// 标刻多点
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        public static string MarkPoints(string DeviceID)
        {
            List<POINTF> lstPoints = new List<POINTF>();
            List<int> lstPtCount = new List<int>();

            int row = 10;
            int col = 10;
            for (int i = 0; i < row; i++)
            {
                for (int ii = 0; ii < col; ii++)
                {
                    POINTF tempPoint;
                    tempPoint.x = i + (float)0.5;
                    tempPoint.y = ii + (float)0.5;
                    lstPoints.Add(tempPoint);
                }
                lstPtCount.Add(col);
            }


            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_MarkPoints func = (BSL_MarkPoints)MarkAPI.Invoke("MarkPoints2", typeof(BSL_MarkPoints));
                if (func != null)
                {
                    BslErrCode iRes = func(DeviceID, lstPoints.ToArray(), lstPoints.Count(), 0);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        message = "标刻完成";
                    }
                    else
                    {
                        message = "标刻失败";
                    }
                }
            }
            return message;
        }

        /// <summary>
        /// 紧急停止
        /// </summary>
        /// <returns></returns>
        public static string EmergenyStop(string DeviceID)
        {
            BSL_EmergenyStop func = (BSL_EmergenyStop)MarkAPI.Invoke("EmergenyStop", typeof(BSL_MarkPoints));
            if (func != null)
            {
                BslErrCode iRes = func(DeviceID);
                if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                {
                    message = "标刻完成";
                }
                else
                {
                    message = "标刻失败";
                }
            }
            return message;
        }
    }
}
