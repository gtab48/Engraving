using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Engraving
{
    //检定数据（理论点<->实际点）
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CALDATA
    {
        public double xT;
        public double yT;
        public double xA;
        public double yA;
    };

    //双精度坐标点
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct POINTDF
    {
        public double x;
        public double y;
    };

    //初始化校正库
    //输入参数: strFileName  文件名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool CLA_InitCalib(string strFileName);

    //根据索引获取校正数据点
    //nSN: 0-3*3点, 1-5*5点, 2-9*9点, 3-17*17点, 4-33*33点, 5-65*65点
    //pnts: 点的理论坐标与实际标刻坐标
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool CAL_GetCalibPnts([MarshalAs(UnmanagedType.LPWStr)] string szFileName, int nSN, [Out] CALDATA[] vPoints);
    //设置指定索引的校正数据点,这个函数里面会完成校正参数的计算
    //nSN: 0-3*3点, 1-5*5点, 2-9*9点, 3-17*17点, 4-33*33点, 5-65*65点
    //pnts: 点的理论坐标与实际标刻坐标
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool CAL_SetCalibPnts([MarshalAs(UnmanagedType.LPWStr)] string szFileName, int nSN, CALDATA[] vPoints);

    //获取等分数据点
    //nAxis: 0-x,1-y
    //nLineCount: 等分线数量
    //pnts: 点的理论坐标与实际标刻坐标, 空间有调用者分配
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool CAL_GetEqualPnts([MarshalAs(UnmanagedType.LPWStr)] string szFileName, int nAxis, ref int nLineCount, [Out] POINTDF[] pnts);
    //设置等分数据点
    //设置指定索引的校正数据点,这个函数里面会完成校正参数的计算
    //nAxis: 0-x,1-y
    //nLineCount: 等分线数量
    //pnts: 点的理论坐标与实际标刻坐标
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool CAL_SetEqualPnts([MarshalAs(UnmanagedType.LPWStr)] string szFileName, int nAxis, int nLineCount, POINTDF[] pnts);

    ////叠加数据
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public class STACKDATAEX
    //{
    //    int nOrder;         //检定行列式阶：3,5,9,17,33,65之一
    //    int nGrid;          //放缩方法:0-按检定点；1-网格比例法
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4225)]
    //    public CALDATA[] vCalPnt;       //检定点数据    //vector<CALDATA> vCalPnt;
    //    double fAxisXL;         //实测X左半区域长度
    //    double fAxisXR;         //实测X右半区域长度
    //    double fAxisYT;         //实测Y上半区域长度
    //    double fAxisYB;         //实测Y下半区域长度
    //    double fOX;             //x轴向偏移
    //    double fOY;             //x轴向偏移
    //    double fSxL;            //X轴左半轴的缩放比，当 nGrid=0时有效
    //    double fSxR;            //X轴右半轴的缩放比，当 nGrid=0时有效
    //    double fSx;             //X轴的缩放比，当 nGrid=0时有效
    //    double fSyT;            //Y轴上半轴的缩放比，当 nGrid=0时有效
    //    double fSyB;            //Y轴下半轴的缩放比，当 nGrid=0时有效
    //    double fSy;             //Y轴的缩放比，当 nGrid=0时有效
    //    double fTxL;            //x左半轴梯形系数
    //    double fTxR;            //x右半轴梯形系数
    //    double fTyT;            //y上半轴梯形系数
    //    double fTyB;            //y下半轴梯形系数
    //    double fPLX;            //x平行形变，added 20181015
    //    double fPLY;            //y平行形变，added 20181015
    //    double fY2XAngle;       //y轴相对于x轴的转向角，added 20181012
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    //    double [] fCK;         //桶形系数
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    //    double [] fSK;          //放缩系数

    //    //每一阶矫正都可以进行等分线矫正
    //    int uLineCount;         //分点数量，包含中心线的数目且应当为奇数
    //                            //等分线方式用的少， 下面的暂不开放
    //                            //vector<double> fXDST;	//分点与中心线的理论距离,当 nGrid=1时有效
    //                            //vector<double> fYDST;	//分点与中心线的理论距离,当 nGrid=1时有效
    //                            //vector<double> fXDSA;	//分点与中心线的实测距离,当 nGrid=1时有效
    //                            //vector<double> fYDSA;	//分点与中心线的实测距离,当 nGrid=1时有效
    //                            //vector<double> fXSR;	//分点百分比,当 nGrid=1时有效
    //                            //vector<double> fYSR;	//分点百分比,当 nGrid=1时有效
    //}
    ////用户校正文件参数结构
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public class CALPAREX
    //{
    //    int calType;            //校正类型: 1-手工校正，2-9点校正
    //    int nStack;             //是否叠加
    //    double fWorkSize;           //振镜扫描区域，不保存
    //    double fValSize;            //有效检定区域
    //    double fZRange;         //Z轴调焦范围 mm
    //    double fTagSize;            //检定点标记尺寸
    //    int XYFLIP;             //XY互换,0= 0-x，1-y； 1= 1-x，0-y,不需保存到文件 by tgf 20180224
    //    int XINVERT;            //X是否反向,不需保存到文件 by tgf 20180224
    //    int YINVERT;            //Y是否反向,不需保存到文件 by tgf 20180224
    //    int nOrder;             //当前检定点行列式的阶：3,5,9,17,33,65之一

    //    //以下是手工校正参数,计算时赋值
    //    double OFFSETX;         //偏移X
    //    double OFFSETY;         //偏移Y
    //    double ROTATEANGLE;     //旋转角度，顺时针为正
    //    double ROTATEOX;        //旋转中心X坐标
    //    double ROTATEOY;        //旋转中心Y坐标
    //    double BUCKETX;         //X桶形失真, 已经左移17位
    //    double BUCKETY;         //Y桶形失真, 已经左移17位
    //    double PRRLX;           //平行四边形失真X
    //    double PRRLY;           //平行四边形失真Y
    //    double TRAPEX;          //梯形失真X
    //    double TRAPEY;          //梯形失真Y
    //    double SCALEX;          //放缩比例X
    //    double SCALEY;          //放缩比例Y

    //    //以下是使用单独配置文件中的参数
    //    bool USE_STANDALONG_MAN_FILE;   // 是否使用单独的配置文件

    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
    //    public byte[] STANDALONG_MAN_FILE;                     // 单独的配置文件路径

    //    // [MarshalAs(UnmanagedType.LPWStr)] string STANDALONG_MAN_FILE;  // 单独的配置文件路径

    //    double BUCKETX_FROMFILE;        // X桶形失真, 已经左移17位，从单独的配置文件中读取的参数
    //    double BUCKETY_FROMFILE;        // Y桶形失真, 已经左移17位，从单独的配置文件中读取的参数
    //    double PRRLX_FROMFILE;          // 平行四边形失真X，从单独的配置文件中读取的参数
    //    double PRRLY_FROMFILE;          // 平行四边形失真Y，从单独的配置文件中读取的参数
    //    double TRAPEX_FROMFILE;         // 梯形失真X，从单独的配置文件中读取的参数
    //    double TRAPEY_FROMFILE;         // 梯形失真Y，从单独的配置文件中读取的参数
    //    double SCALEX_FROMFILE;         // 放缩比例X，从单独的配置文件中读取的参数
    //    double SCALEY_FROMFILE;         // 放缩比例Y，从单独的配置文件中读取的参数
    //                                    //以上是使用单独配置文件中的参数

    //    int nGrid;              //是否等分
    //    int uLineCount;         //分点数量，包含中心线的数目且应当为奇数
    //                            //等分线方式用的少， 下面的暂不开放
    //                            //vector<double> fXDST;	//分点与中心线的理论距离,当 nGrid=1时有效
    //                            //vector<double> fYDST;	//分点与中心线的理论距离,当 nGrid=1时有效
    //                            //vector<double> fXDSA;	//分点与中心线的实测距离,当 nGrid=1时有效
    //                            //vector<double> fYDSA;	//分点与中心线的实测距离,当 nGrid=1时有效
    //                            //vector<double> fXSR;	//分点百分比,当 nGrid=1时有效
    //                            //vector<double> fYSR;	//分点百分比,当 nGrid=1时有效
    //                            //以上是手工校正参数
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    //    public STACKDATAEX[] calDatas;//各阶（3,5,9,17,33,65）基本检定数据  //#define ORDERS  7
    //}
    //[UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    //public delegate bool CAL_LoadUcf([MarshalAs(UnmanagedType.LPWStr)] string szFileName, [Out] CALPAREX param);

    ////保存用户校正文件
    //[UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    //public delegate bool CAL_SaveUcf([MarshalAs(UnmanagedType.LPWStr)] string szFileName, [Out] CALPAREX param);

    /*
     * 获取校正的基本参数
     * fWorksize-工作区域
     * fValSize-校正区域
     * fTagSize-校正点十字尺寸
     * nLineCount - 等分线数量
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool CAL_GetBasePara([MarshalAs(UnmanagedType.LPWStr)] string szFileName, ref double fWorksize, ref double fValSize, ref double fTagSize, ref int nLineCount);
}
