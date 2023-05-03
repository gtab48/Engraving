using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Engraving
{
    internal class MarkAPI
    {
        #region 通用宏定义
        public enum BslErrCode
        {
            BSL_ERR_WRONGPARAM = -1, //传入的参数错误
            BSL_ERR_SUCCESS = 0,            //成功
            BSL_ERR_BSLCADRUN = 1,          //发现BslCAD在运行
            BSL_ERR_NOFINDCFGFILE = 2,      //找不到BslCAD.CFG
            BSL_ERR_FAILEDOPEN = 3,         //打开LMC1失败
            BSL_ERR_NODEVICE = 4,           //没有有效的lmc1设备
            BSL_ERR_HARDVER = 5,            //lmc1版本错误
            BSL_ERR_DEVCFG = 6,             //找不到设备配置文件
            BSL_ERR_STOPSIGNAL = 7,         //报警信号
            BSL_ERR_USERSTOP = 8,           //用户停止
            BSL_ERR_UNKNOW = 9,             //不明错误
            BSL_ERR_OUTTIME = 10,           //超时
            BSL_ERR_NOINITIAL = 11,         //未初始化
            BSL_ERR_READFILE = 12,          //读文件错误
            BSL_ERR_OWRWNDNULL = 13,        //窗口为空
            BSL_ERR_NOFINDFONT = 14,        //找不到指定名称的字体
            BSL_ERR_PENNO = 15,             //错误的笔号
            BSL_ERR_NOTTEXT = 16,           //指定名称的对象不是文本对象
            BSL_ERR_SAVEFILE = 17,          //保存文件失败
            BSL_ERR_NOFINDENT = 18,         //找不到指定对象
            BSL_ERR_STATUE = 19,            //当前状态下不能执行此操作
            BSL_ERR_LOADNEWFILE = 20, //加载振镜校正文件失败
            BSL_ERR_INCORRECTCALPOINT = 21,  //检定点数不正确，必须超过3x3点
            BSL_ERR_INCORRECTFILELINE = 22,  //文件行数不对

            BSL_ERR_OPENVEC_FAIL = 100, //打开向量文件失败
        }
        // public static const int MAX_LENGTH;
        public class BSL_DEFINE
        {
            public const int MAX_SHAPE_COUNT = 256; //最大图元数量
            public const int BSL_BUFFER_SIZE = 256;//缓冲区大小
            public const double PI = 3.1415926;     //圆周率
            public const int MAX_SHAPE_FILL_COUNT = 4; //最大支持四层填充
            //public const int MAX_SHAPE_LINE_COUNT = 1000; //最大支持路径行数
            //public const int MAX_SHAPE_POINT_COUNT = 5000; //每行最大支持点数
            public const int MAX_SHAPE_LINE_COUNT = 500; //最大支持路径行数
            public const int MAX_SHAPE_POINT_COUNT = 500; //每行最大支持点数
        }
        public enum BARCODETYPE
        {
            BARCODETYPE_39 = 0,
            BARCODETYPE_93 = 1,
            BARCODETYPE_128A = 2,
            BARCODETYPE_128B = 3,
            BARCODETYPE_128C = 4,
            BARCODETYPE_128OPT = 5,
            BARCODETYPE_EAN128A = 6,
            BARCODETYPE_EAN128B = 7,
            BARCODETYPE_EAN128C = 8,
            BARCODETYPE_EAN13 = 9,
            BARCODETYPE_EAN8 = 10,
            BARCODETYPE_UPCA = 11,
            BARCODETYPE_UPCE = 12,
            BARCODETYPE_25 = 13,
            BARCODETYPE_INTER25 = 14,
            BARCODETYPE_CODABAR = 15,
            BARCODETYPE_PDF417 = 16,
            BARCODETYPE_DATAMTX = 17,
            BARCODETYPE_USERDEF = 18
        }

        public enum BARCODEATTRIB
        {
            BARCODEATTRIB_REVERSE = 0x0008, //条码反转
            BARCODEATTRIB_HUMANREAD = 0x1000, //显示人识别字符
            BARCODEATTRIB_CHECKNUM = 0x0004, //需要校验码
            BARCODEATTRIB_PDF417_SHORTMODE = 0x0040, //PDF417为缩短模式
            BARCODEATTRIB_DATAMTX_DOTMODE = 0x0080, //DataMtrix为点模式
            BARCODEATTRIB_CIRCLEMODE = 0x0100 //自定义二维码为圆模式
        }

        public enum DATAMTX_SIZEMODE
        {
            DATAMTX_SIZEMODE_SMALLEST = 0,
            DATAMTX_SIZEMODE_10X10 = 1,
            DATAMTX_SIZEMODE_12X12 = 2,
            DATAMTX_SIZEMODE_14X14 = 3,
            DATAMTX_SIZEMODE_16X16 = 4,
            DATAMTX_SIZEMODE_18X18 = 5,
            DATAMTX_SIZEMODE_20X20 = 6,
            DATAMTX_SIZEMODE_22X22 = 7,
            DATAMTX_SIZEMODE_24X24 = 8,
            DATAMTX_SIZEMODE_26X26 = 9,
            DATAMTX_SIZEMODE_32X32 = 10,
            DATAMTX_SIZEMODE_36X36 = 11,
            DATAMTX_SIZEMODE_40X40 = 12,
            DATAMTX_SIZEMODE_44X44 = 13,
            DATAMTX_SIZEMODE_48X48 = 14,
            DATAMTX_SIZEMODE_52X52 = 15,
            DATAMTX_SIZEMODE_64X64 = 16,
            DATAMTX_SIZEMODE_72X72 = 17,
            DATAMTX_SIZEMODE_80X80 = 18,
            DATAMTX_SIZEMODE_88X88 = 19,
            DATAMTX_SIZEMODE_96X96 = 20,
            DATAMTX_SIZEMODE_104X104 = 21,
            DATAMTX_SIZEMODE_120X120 = 22,
            DATAMTX_SIZEMODE_132X132 = 23,
            DATAMTX_SIZEMODE_144X144 = 24,
            DATAMTX_SIZEMODE_8X18 = 5,
            DATAMTX_SIZEMODE_8X32 = 6,
            DATAMTX_SIZEMODE_12X26 = 27,
            DATAMTX_SIZEMODE_12X36 = 28,
            DATAMTX_SIZEMODE_16X36 = 29,
            DATAMTX_SIZEMODE_16X48 = 30
        }

        //填充类型
        public enum BSL_FILLTYPE
        {
            BSL_FT_CIRCULAR = 0,        /* 环形填充 */
            BSL_FT_SINGLE_LINE,         /* 弓形, 在单个连通区是不断开 */
            BSL_FT_SINGLE_LINE_BREAK,   /* 弓形，跨越两个不相连的图块时，中间会断开*/
            BSL_FT_MULTI_LINE,          /* 多线，单向填充，各线段在两端不相连*/
            BSL_FT_MULTI_LINE_TWO_DIR   /* 多线，双向填充，各线段在两端不相连，可减少空跳时间*/
        };

        public enum BLS_FONTTYPE
        {
            FT_TRUETYPE = 0,
            FT_SINGLELINE,
            FT_CODEBAR,
            FT_PTMATRIC//,      //点阵字体 
        };
        #endregion

        #region 通用结构体

        //图元填充参数
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct BSL_ShapeFillPara
        {
            public BSL_FILLTYPE m_nFillType;    //填充类型, 
            public int m_nExecuteType;      //多个图形的运算方式，0=异或 1=交  2=并 3=差，
            public int m_nIndex;            //第几个填充
            public bool m_bEnable;          //使能当前填充
            public bool m_bRoundInvert;

            public double m_fLineSpacing;   //线间距
            public double m_fMargin;        // 边距

            public bool m_fill_rotate;      //自动旋转角度标刻
            public double m_fRotateAngle;   //旋转角度

            public double m_fstrFillGraduallyMaxs;  //渐变区域填充线最大间距
            public float m_fstrFillGraduallyRange;      //渐变区域设置填充宽度
            public double m_fstrFillGraduallyRate; //渐变区域变化幅度
            public float m_fstrFillGraduallyActualRange; //渐变区域实际宽度
            public bool m_bEnableFillGradually;     //使能渐变填充

            // 以下属性对环形填充无效的
            public bool m_bWholeConsider;  /* 整体计算，当环形填充时无效*/
            public bool m_bAlongBorder; /* 绕边走一次，当环形填充时无效*/
            public bool m_bCrossFill; //交叉填充, by tgf 20180410
            public bool m_bQuickFill; //快速填充(用于单线单向与单线双向), by tgf 20180702
            public double m_fAngle;  // 填充线旋转的角度(弧度值)，对环形无效
            public UInt32 m_nFillMarkCount;// 填充线标刻次数
            public int m_nPenNum;      //笔号
            public UInt32 m_cPenColor; //颜色
            public UInt32 m_nCircularCount; // 边界环数，是除绕边走一圈以外的的环,对环形填充无效
            public double m_fCircularGap;  // 环间距，
            public double m_fInnerSpacing;   //直线缩进，是环与绕边走一圈里面的
                                             // 注意：环与绕边走一圈的区别：绕边走一圈与填充线条是没有间距的。	
            public bool m_bArrangeEqually;  //平均分布各条，如果为false,则以下属性有效
            public double m_fStartPreserve; // 开始保留
            public double m_fEndPreserve;       //结束保留

            public void init()
            {
                m_bEnable = false;
                m_nFillType = BSL_FILLTYPE.BSL_FT_CIRCULAR;
                m_nExecuteType = 0;
                m_bWholeConsider = true;
                m_bAlongBorder = false;
                m_bCrossFill = false;
                m_bQuickFill = false;
                m_fAngle = 0;
                m_nFillMarkCount = 1;
                m_nPenNum = 0;
                m_fLineSpacing = 0.06;
                m_bArrangeEqually = false;
                m_fMargin = 0;
                m_fStartPreserve = 0;
                m_fEndPreserve = 0;
                m_fInnerSpacing = 0;
                m_nCircularCount = 0;
                m_fCircularGap = 0.5;
                m_bRoundInvert = false;
                m_cPenColor = 0;         // RGB(0, 0, 0);
                m_fill_rotate = false;
                m_fRotateAngle = 0;
                m_fstrFillGraduallyMaxs = 0.06;
                m_fstrFillGraduallyRange = 6;
                m_fstrFillGraduallyRate = 0.006;
                m_fstrFillGraduallyActualRange = 6;
            }
        };

        //填充参数
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct BSL_FillPara
        {
            /*使能轮廓和/或绕边走一次：
             * 0:无轮廓，无绕边走一次；1:有轮廓无绕边；2：无轮廓，有绕边；3:有轮廓有绕边
             * 当有轮廓且优先轮廓时，轮廓在填充线前，若不优先轮廓，则轮廓在填充线后；
             * 绕边一次的标刻，总是在填充之后。
             */
            public int m_bOutLine;
            public bool m_bOutLineFirst;    //是否先标刻轮廓
            public bool m_bKeepSeperate;   //保持填充对象的独立
            public bool m_bDelUngroup;      //删除填充时是否解散群组
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.MAX_SHAPE_FILL_COUNT)]
            public BSL_ShapeFillPara[] m_arrPar;   //多组参数
            public void init()
            {
                m_bOutLine = 1;
                m_bOutLineFirst = true;
                m_bKeepSeperate = false;
                m_bDelUngroup = true;
                bool bFirstParEnabled = true;

                m_arrPar = new BSL_ShapeFillPara[BSL_DEFINE.MAX_SHAPE_FILL_COUNT];
                for (int i = 0; i < BSL_DEFINE.MAX_SHAPE_FILL_COUNT; i++)
                {
                    m_arrPar[i].init();
                    m_arrPar[i].m_bEnable = bFirstParEnabled;
                    m_arrPar[i].m_nPenNum = 0;
                    m_arrPar[i].m_nIndex = i;
                    bFirstParEnabled = false;
                }
            }
        };

        //设备ID
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STU_DEVID
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public byte[] wszDevName;
        };
        //图元信息
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct ShapeInfo2
        {
            public UInt32 nShapeIndex;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
            public byte[] wszShapeName;
            public int iShapeType;
        };
        //图元名称
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct ShapeName
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
            public byte[] wszShapeName;
        };
        //参数
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STU_PARA
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public byte[] wszParaName;
        };

        //二维码类型
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STU_BARCODETYPE
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public byte[] wszParaName;
        };
        //点
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct POINTF
        {
            public float x;
            public float y;
        };


        //路径点数据
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LineDataShape
        {
            public int nPtCount;    //行数   点数

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.MAX_SHAPE_POINT_COUNT)]
            public POINTF[] pPoint;   //点

            /// <summary>
            /// 初始化
            /// </summary>
            public void Init()
            {
                this.nPtCount = 0;

                for (int i = 0; i < BSL_DEFINE.MAX_SHAPE_POINT_COUNT; i++)
                {
                    pPoint[i].x = 0.0F;
                    pPoint[i].y = 0.0F;
                }
            }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="nCnt">点阵数</param>
            public LineDataShape(int nCnt)
            {
                this.nPtCount = nCnt;

                pPoint = new POINTF[BSL_DEFINE.MAX_SHAPE_POINT_COUNT];

                Init();
            }
        };

        //路径数据
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PathDataShape
        {
            public int nPenIdx;

            public int nMarkCount;

            public int nLineCount;    //行数   路径数

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.MAX_SHAPE_LINE_COUNT)]
            public LineDataShape[] pLine;

            /// <summary>
            /// 初始化
            /// </summary>
            public void Init()
            {
                nLineCount = 0;

                for (int i = 0; i < BSL_DEFINE.MAX_SHAPE_LINE_COUNT; i++)
                {
                    pLine[i].Init();
                }
            }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="nCnt">行数</param>
            public PathDataShape(int nCnt)
            {
                nLineCount = nCnt;
                this.nPenIdx = 0;
                this.nMarkCount = 0;

                pLine = new LineDataShape[BSL_DEFINE.MAX_SHAPE_LINE_COUNT];
                for (int i = 0; i < BSL_DEFINE.MAX_SHAPE_LINE_COUNT; i++)
                {
                    pLine[i] = new LineDataShape(0);
                }
                Init();
            }
        };
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        //网格检定数据
        public struct gridCalData
        {
            public double xT;   //理论点X
            public double yT;   //理论点Y
            public double xA;   //实测点X
            public double yA;   //实测点Y
            /// <summary>
            /// 初始化
            /// </summary>
            public void Init(gridCalData src)
            {
                xT = src.xT;
                yT = src.yT;
                xA = src.xA;
                yA = src.yA;
            }
            ///<summary>
            ///构造函数
            ///</summary>
            ///<param name="nCnt">行数</param>
            public gridCalData(gridCalData src)
            {
                xT = 0.0F;
                yT = 0.0F;
                xA = 0.0F;
                yA = 0.0F;
                Init(src);
            }

        };
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct PenPar
        {

            /// int
            public int iSwitchOn;

            /// char[256]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string cName;

            /// char[256]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string wcName;

            /// UINT->unsigned int
            public uint nMachineCount;

            /// UINT->unsigned int
            public uint nRunSpeed;

            /// UINT->unsigned int
            public uint nLaserPower;

            /// int
            public int nEffectWaitTime;

            /// float
            public float fCurrent;

            /// float
            public float fLaserFreq;

            /// float
            public float fLaserZkb;

            /// int
            public int nMopaPulse;

            /// int
            public int iOpenDelay;

            /// int
            public int iShutDelay;

            /// int
            public int iEndDelay;

            /// int
            public int iCornerDelay;

            /// int
            public int iStepDelay;

            /// float
            public float fStepLen;

            /// boolean
            public bool bChangeOfCornerDelay;

            /// int
            public int nVoltADOutput;

            /// int
            public int iDAAdvance;

            /// float
            public float fOverlapLen;

            /// float
            public float fAccLen;

            /// float
            public float fDecLen;

            /// float
            public float fDAPowerSt;

            /// float
            public float fDAPowerEnd;

            /// float
            public float fPWMZkbSt;

            /// float
            public float fPWMZkbEnd;

            /// COLORREF->DWORD->unsigned int
            public uint cPenColor;

            /// UINT->unsigned int
            public uint JUMPSPEED;

            /// UINT->unsigned int
            public uint JUMPPOSITONDELAY;

            /// UINT->unsigned int
            public uint JUMPDISTANCEDELAY;

            /// UINT->unsigned int
            public uint ENDCOMP;

            /// UINT->unsigned int
            public uint ACCDISTANCE;

            /// float
            public float POINTTIME;

            /// BOOL->int
            public int PULSEPOINTMODE;

            /// UINT->unsigned int
            public uint PULSEPERPOINT;

            /// BOOL->int
            public int WOBBLE;

            /// float
            public float WOBBLEDIAMETER;

            /// float
            public float WOBBLEDISTANCE;

            /// BOOL->int
            public int ENDADDPOINT;

            /// UINT->unsigned int
            public uint ADDCOUNT;

            /// float
            public float ADDPOINTDISTANCE;

            /// float
            public float ADDPOINTTIME;

            /// UINT->unsigned int
            public uint ADDPOINTCYCLES;

            /// UINT->unsigned int
            public uint OPENLIGHTBEHIND;

            /// UINT->unsigned int
            public uint JUMPMINDELAY;

            /// UINT->unsigned int
            public uint JUMPMAXDELAY;

            /// UINT->unsigned int
            public uint JUMPMAXDISTANCE;
        }
        #endregion

        #region 设备
        //初始化全部lmc控制卡
        //hOwrWnd 指拥有用户输入焦点的窗口，用于检测用户暂停消息
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_InitDevices(Int32 dwWndHanle);

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ReInitDevices([MarshalAs(UnmanagedType.LPWStr)] string strDevID, bool bAdd);

        //启动UDP识别
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_UDPInit(IntPtr dwWndHanle, int Msg);

        //UDP识别
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_UDPDeviceChanged(IntPtr wParam, IntPtr lParam);


        //重置全部控制卡，将重新初始化所有的控制卡
        //hOwrWnd 指拥有用户输入焦点的窗口，用于检测用户暂停消息
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ResetDevices(Int32 dwWndHanle);


        //获取所有的设备ID
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetAllDevices([Out] STU_DEVID[] vDevID, ref int nDevCount);

        //获取二维码类型列表
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetTBarCodeType([Out] STU_BARCODETYPE[] vDevID, ref int nTypeCount);

        //获取所有参数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetAllPara([Out] STU_PARA[] vDevID, ref int nParCount);


        //关联设备与参数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AssocDevPara([MarshalAs(UnmanagedType.LPWStr)] string strDevID, [MarshalAs(UnmanagedType.LPWStr)] string strParName);

        //获取关联设备的参数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetAssocParaNameByDevID([MarshalAs(UnmanagedType.LPWStr)] string strDevID, char[] strParName);

        //打开所有控制卡
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_OpenAllDevice();

        //打开特定ID的控制卡
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_OpenDevice([MarshalAs(UnmanagedType.LPWStr)] string strDevID);

        //关闭所有控制卡
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_CloseAllDevice();

        //关闭特定ID的控制卡
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_CloseDevice([MarshalAs(UnmanagedType.LPWStr)] string strDevID);

        #endregion

        #region 文件

        //显示九点校正 iType=1九点校正
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate void BSL_ShowCaliDlg(int iType, [MarshalAs(UnmanagedType.LPWStr)] string strParName);

        //预览图形文件 供c#调用
        //输入参数: strFileName  图形文件名称
        //			hWnd 图形显示的窗口
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_DrawFileInWnd2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 dwWndHanle, bool bDrawAxis);

        //预览图形文件 供c#调用
        //输入参数: strFileName  图形文件名称
        //			hWnd 图形显示的窗口
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate IntPtr BSL_DrawFileInImg([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 dwWidth, Int32 dwHeight, bool bDrawAxis);


        //预览图形文件-----bDrawGlvArea为：true最大显示振镜区域，false为最大图形显示。并支持缩放
        //输入参数: strFileName  图形文件名称
        //			nWidth nHeight 图形显示的范围大小
        //          fDelta 放缩比，调节放缩的尺寸
        //          bDrawGlvArea  是否显示振镜区域线
        //			bDrawAxis   是否绘制轴线 
        //			nBlankSize  两边留白像素点
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate IntPtr BSL_DrawFileInImgEx([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 dwWidth, Int32 dwHeight, float fDelta, bool bDrawGlvArea, bool bDrawAxis, UInt32 nBlankSize, bool bDrawSelectRect);


        //加载图形文件
        //输入参数: strFileName  图形文件名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_LoadDataFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName);


        //新建一个orzx文档文件
        //输入参数: strFileName  图形文件名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_NewFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName);


        //加载矢量文件
        //输入参数: strFileName  矢量文件名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_LoadVectorFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName, bool bIsCenter);

        // 替换矢量文件
        // 输入参数: strFileName 文件名
        // 输入参数: strVectorFileName 矢量文件名
        // 输入参数: strEntName 图元实体名
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ReplaceVectorFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strVectorFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName);


        //设置是否使用校正文件
        //输入参数: bEnable - 是否使能校正文件
        //输入参数: strParName  将要设置的参数名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate void BSL_SetCalFileFlag(bool bEnable, [MarshalAs(UnmanagedType.LPWStr)] string strParName);


        //设置校正文件
        //输入参数: strFileName  校正文件名
        //输入参数: strParName  将要设置的参数名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate void BSL_SetCalFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strParName);

        //设置XY反向标志
        //输入参数: iXYFlip:是否xy互换 iXInvert:是否x反向 iYInvert:是否y反向
        //输入参数: strParName  将要设置的参数名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate void BSL_SetXYFlipFlag(int iXYFlip, int iXInvert, int iYInvert, [MarshalAs(UnmanagedType.LPWStr)] string strParName);


        //获取某个打开的文件中的图形列表
        //输入参数: strFileName  图形文件名称
        //			vShapes 图形信息容器
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetShapesInFile2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [Out] ShapeInfo2[] shapes, int nMaxShapeSize);


        //卸载图形文件
        //输入参数: strFileName  图形文件名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_UnLoadDataFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName);



        //将图形文件附加到设备
        //输入参数: strFileName  图形文件名称
        //			strDevID 设备ID
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AppendFileToDevice([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strDevID);

        //将图形文件从设备解除关联
        //输入参数: strFileName  图形文件名称
        //			strDevID 设备ID
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_UnappendFileFromDevice([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strDevID);



        public struct STU_FILENAME
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
            public byte[] wszParaName;
        };
        //获取设备的关联图形文件列表
        //输入参数: vFileName  图形文件名称列表
        //			strDevID 设备ID
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetAssocFilesOfDevice([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [Out] STU_FILENAME[] vFileName, ref int nNameCount);

        #endregion

        #region 标刻

        //标刻选定的设备的关联数据文件
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkByDeviceId([MarshalAs(UnmanagedType.LPWStr)] string strDevID);

        //标刻选定的设备的关联数据文件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDevID">板卡ID</param>
        /// <param name="nMarkCount">标刻总数</param>
        /// <param name="bContinue">是否连续标刻</param>
        /// <param name="nTriggerPort">硬件触发端口号</param>
        /// <param name="nTriggerLevel">硬件触发电平</param>
        /// <returns></returns>
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkByDeviceIdOfHardwareTrigger([MarshalAs(UnmanagedType.LPWStr)] string strDevID, int nMarkCount, bool bContinue, int nTriggerPort, int nTriggerLevel);

        //标刻当前数据库里的所有数据
        //输入参数: bFlyMark = true 使能飞动打标  bFlyMark = false 使能飞动打标
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_Mark([MarshalAs(UnmanagedType.LPWStr)] string strDevID, bool bFlyMark);

        //停止标刻
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_StopMark([MarshalAs(UnmanagedType.LPWStr)] string strDevID);

        //标刻当前数据库里的指定对象
        //输入参数: strEntName 要加工的指定对象的名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkEntity([MarshalAs(UnmanagedType.LPWStr)] string strDevID, [MarshalAs(UnmanagedType.LPWStr)] string szFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName);

        //飞动标刻当前数据库里的指定对象
        //输入参数: strEntName 飞动打标指定对象的名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkEntityFly([MarshalAs(UnmanagedType.LPWStr)] string strDevID, [MarshalAs(UnmanagedType.LPWStr)] string szFileName);


        // 读lmc1的输入端口
        //输入参数: 读入的输入端口的数据
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ReadPort([MarshalAs(UnmanagedType.LPWStr)] string strDevID, ref UInt16 data);

        // 读lmc1的输出端口
        //输入参数: 读入的输出端口的数据
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ReadOutPort([MarshalAs(UnmanagedType.LPWStr)] string strDevID, ref UInt16 data);

        //使用一个设备显示红光
        //szDevId-设备ID
        //bCountinue - 是否连续显示红光
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_RedLightMark([MarshalAs(UnmanagedType.LPWStr)] string strDevID, bool bCountinue);

        //使用一个设备显示红光
        //szDevId-设备ID
        //输入参数: strEntName 要加工的指定对象的名称
        //int[] vShpIndex 要红光的图元序号
        //int iShpCount 要红光的图元个数
        //bCountinue - 是否连续显示红光
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_RedLightMarkByEnt2([MarshalAs(UnmanagedType.LPWStr)] string strDevID, [MarshalAs(UnmanagedType.LPWStr)] string szFileName, int[] vShpIndex, int iShpCount, bool bCountinue);

        //移动振镜
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GoToPos([MarshalAs(UnmanagedType.LPWStr)] string strDevID, double x1, double y1);

        //标刻线
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkLine([MarshalAs(UnmanagedType.LPWStr)] string strDevID, double x1, double y1, double x2, double y2, int pen);

        //标刻点
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkPoint([MarshalAs(UnmanagedType.LPWStr)] string strDevID, double x, double y, double delay, int pen);

        //标刻一组点
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkPoints([MarshalAs(UnmanagedType.LPWStr)] string strDevID, POINTF[] vPoints, int iPtCount, int nPenNum);

        //标刻一组线段 统一笔号
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkLines([MarshalAs(UnmanagedType.LPWStr)] string strDevID, POINTF[] lines, int iLineCount, int[] iPtCount, int penNum);

        //标刻一组线段 笔号penNum是个数组
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkLinesX([MarshalAs(UnmanagedType.LPWStr)] string strDevID, POINTF[] lines, int iLineCount, int[] iPtCount, int[] penNum);

        //生成填充线
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetFillData(POINTF[] outline, int[] outlinepnts, int outlinecount, BSL_FillPara fillPar, [Out] POINTF[] fillLines, ref int filllinecount, [Out] int[] filllinepnts);

        //分块标刻图形文件中的图形对象
        //标刻的范围按所有图形的最大外接矩形大小，标刻时会自动将分块中心与振镜中心对齐
        //输入参数: szDevId 设备ID
        //          szDocName 要加工的文件名称
        //			dx-分块的宽度
        //			dy-分块的高度
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkBlockByDoc([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string szDocName, double dx, double dy);


        //获取标刻路径数据
        /**
          * 获取文件中所有打标路径数据 for C#
          * szDevId：设备ID
          * szDocName: 文件名
          * bRotOffset: 是否做偏移旋转
          * r   旋转角度
          * dx  X偏移
          * dy  Y偏移
          * cx  旋转中心X坐标
          * cy  旋转中心Y坐标
          * nCount 输出数目
          * pMarkPaths：输出标刻数据
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetMarkDataPaths2([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string szDocName,
             bool bRotOffset, double r, double dx, double dy, double cx, double cy,
             ref int nCount, IntPtr pMarkPaths, ref PenPar penpar);

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetMarkDataPaths3([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string szDocName,
             bool bRotOffset, double r, double dx, double dy, double cx, double cy,
             ref int nCount, [Out] PathDataShape[] MarkPaths, ref PenPar penpar);

        //标刻路径
        /**
          * 标刻路径点 for C#
          * szDevId：设备ID
          * nCount 数目
          * pMarkPaths：标刻数据
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkDocDataPaths2([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int nCount, IntPtr pMarkPaths, PenPar penpar, [MarshalAs(UnmanagedType.LPWStr)] string szParName);

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkDocDataPaths3([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int nCount, PathDataShape[] pMarkPaths, PenPar penpar, [MarshalAs(UnmanagedType.LPWStr)] string szParName);


        /**获取模板文件图元标刻轮廓和填充路径
         * 输入参数： szDocName --图形文件名称
         * 输入参数： iShapeIndex --对象在图形列表中的索引号  iShapeIndex = -1 获取文件所有图元轮廓和填充路径
         * 输出参数： nOutCount 轮廓数
         * 输出参数： pOutPaths --轮廓路径
         * 输出参数： nFillCount 填充数
         * 输出参数： pFillPaths --填充路径
         */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetShapeMarkPaths2([MarshalAs(UnmanagedType.LPWStr)] string szDocName, int iShapeIndex,
             ref int nOutCount, [Out] PathDataShape[] pOutPaths, ref int nFillCount, [Out] PathDataShape[] pFillPaths);

        /** 路径标刻
          * 输入参数： szDevId --设备ID
          * 输入参数： nCount --路径数
          * 输入参数： pMarkPaths --标刻数据
          * 输入参数： nPenNo --笔号参数
          * 输入参数： szParName --配置参数名称
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkShapePaths2([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int nCount, PathDataShape[] pMarkPaths, int nPenNo, [MarshalAs(UnmanagedType.LPWStr)] string szParName);


        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_LoadNewCalFile2([Out] STU_FILENAME[] vFileName, ref int iNameCount);

        //保存校正文件
        //输入参数: strCalFileName  校正文件路径
        //输入参数: nIndex			校正点数列表当前索引    0: 3*3   1: 5*5   2: 9*9   3: 17*17   4: 33*33   5: 65*65    6: 25*25
        //输入参数: vCalData		网格检定点理论和实测数据
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SaveCalFile2([MarshalAs(UnmanagedType.LPWStr)] string strCalFileName, int nIndex, gridCalData[] pCalData, int nDataCount);

        //标刻九点矩形
        //输入参数: szDevId			设备ID
        //输入参数: strCalFileName  校正文件路径
        //输入参数: fValSize		有效检定区域 mm
        //输入参数: fTagSize		校正标记尺寸 mm
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkNinePointRect([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string strCalFileName, double fValSize, double fTagSize);

        //标刻最大图形
        //输入参数: szDevId			设备ID
        //输入参数: strCalFileName  校正文件路径
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkMaxShape([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string strCalFileName);

        //多点校正标刻
        //输入参数: szDevId			设备ID
        //输入参数: strCalFileName  校正文件路径
        //输入参数: fValSize		有效检定区域 mm
        //输入参数: fTagSize		校正标记尺寸 mm
        //输入参数: nIndex			校正点数列表当前索引    0: 3*3   1: 5*5   2: 9*9   3: 17*17   4: 33*33   5: 65*65    6: 25*25
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkMultiCalPoint([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string strCalFileName, double fValSize, double fTagSize, int nIndex);

        //导入测量值
        //输入参数: nIndex			校正点数列表当前索引    0: 3*3   1: 5*5   2: 9*9   3: 17*17   4: 33*33   5: 65*65    6: 25*25
        //输出参数: vCalData		网格检定点实测数据
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_LoadMeasuredValueFile2(int nIndex, [Out] gridCalData[] pCalData, ref int nDataCount);

        //标刻验证图形
        //输入参数: szDevId			设备ID
        //输入参数: strCalFileName  校正文件路径
        //输入参数: nIndex			校正点数列表当前索引    0: 3*3   1: 5*5   2: 9*9   3: 17*17   4: 33*33   5: 65*65    6: 25*25
        //输入参数: nGap			线框间隔 mm  min(nGap, 5.0)  一般取5   
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkCheckShape([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string strCalFileName, int nIndex, double nGap);
        #endregion


        #region 端口
        // 写lmc1的输出端口
        //输入参数: 
        // szDevId - 设备ID
        // portNum - 输出端口, 目前有效端口为0-7
        // nMode - 输出模式, 0-电平，1-脉冲，2-跳变
        // nLevel- 输出电平, 0-低电平，1-高电平
        // nPulse - 脉冲周期 ,us， 0-65535us,当 nMode=1脉冲模式时有效
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_WritePort([MarshalAs(UnmanagedType.LPWStr)] string strDevID, UInt16 portNum, UInt16 nMode, UInt16 nLevel, UInt16 nPulse);


        //获取所有输出口状态
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetOutPort([MarshalAs(UnmanagedType.LPWStr)] string strDevID, ref Int16 status);

        //向指定的IP和端口发送字符串
        //输入参数: m_dwIpAddress	IP地址
        //			m_strPort       端口号
        //          m_dwConnectTimeout     连接超时
        //          m_dwReceiveTimeout     接收超时
        //          sendStr                发送内容
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SendStrByTcpIp(UInt32 m_dwIpAddress, [MarshalAs(UnmanagedType.LPWStr)] string m_strPort, UInt32 m_dwConnectTimeout, UInt32 m_dwReceiveTimeout, [MarshalAs(UnmanagedType.LPWStr)] string sendStr);

        #endregion

        #region 笔号

        //得到指定笔号对应的加工参数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetPenParam([MarshalAs(UnmanagedType.LPWStr)] string strFileName,  //文件名称
                UInt32 nPenNo,                     //要读取的笔号(0-255)	
                ref int nMarkLoop,              //加工次数
                ref double dMarkSpeed,          //标刻次数mm/s
                ref double dPowerRatio,         //功率百分比(0-100%)	
                ref double dCurrent,            //电流A
                ref int nFreq,                  //频率HZ
                ref int nQPulseWidth,           //Q脉冲宽度us	
                ref int nStartTC,               //开始延时us
                ref int nLaserOffTC,            //激光关闭延时us
                ref int nEndTC,                 //结束延时us
                ref int nPolyTC,                //拐角延时us
                ref double dJumpSpeed,          //跳转速度mm/s
                ref int nJumpPosTC,             //跳转位置延时us 
                ref int nJumpDistTC,            //跳转距离延时us	
                ref double dEndComp,            //末点补偿mm				
                ref bool bPulsePointMode,       //脉冲点模式 
                ref int nPulseNum,              //脉冲点数目
                ref float POINTTIME);           // 打点时间

        //设置指定笔号对应的加工参数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SetPenParam([MarshalAs(UnmanagedType.LPWStr)] string strFileName, //文件名称
                UInt32 nPenNo,                  //要设置的笔号(0-255)	
                int nMarkLoop,              //加工次数
                double dMarkSpeed,          //标刻次数mm/s
                double dPowerRatio,         //功率百分比(0-100%)	
                double dCurrent,            //电流A
                int nFreq,                  //频率HZ
                int nQPulseWidth,        //Q脉冲宽度us	
                int nStartTC,               //开始延时us
                int nLaserOffTC,            //激光关闭延时us
                int nEndTC,                 //结束延时us
                int nPolyTC,                //拐角延时us
                double dJumpSpeed,          //跳转速度mm/s
                int nJumpPosTC,             //跳转位置延时us 
                int nJumpDistTC,            //跳转距离延时us	
                double dEndComp,            //末点补偿mm				
                bool bPulsePointMode,       //脉冲点模式 
                int nPulseNum,              //脉冲点数目
                float POINTTIME);           // 打点时间

        //设置对象笔号
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SetShapePenNo([MarshalAs(UnmanagedType.LPWStr)] string strFileName, /*打开的文件*/
                        [MarshalAs(UnmanagedType.LPWStr)] string szEntName,  /*对象名称*/
                        UInt32 nPenNo); /*笔号*/


        #endregion

        #region 扩展轴
        //扩展轴函数
        //扩展轴移动到指定坐标位置
        //输入参数:  szDevId=设备ID,
        //          axis     扩展轴  0 = 轴0  1 = 轴1
        //          GoalPos  坐标位置
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AxisMoveTo([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis, double GoalPos);

        //扩展轴校正原点
        //输入参数:  szDevId=设备ID,axis     扩展轴  0 = 轴0  1 = 轴1
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AxisCorrectOrigin([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis);

        //得到扩展轴的当前坐标
        //输入参数:  szDevId=设备ID,axis扩展轴  0 = 轴0  1 = 轴1
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate double BSL_GetAxisCoor([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis);

        //扩展轴移动到指定脉冲坐标位置
        //输入参数: szDevId=设备ID,
        //          axis     扩展轴  0 = 轴0  1 = 轴1
        //          nGoalPos  脉冲坐标位置
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AxisMoveToPulse([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis, double GoalPos);

        //得到扩展轴的当前脉冲坐标
        //输入参数:  szDevId=设备ID, axis扩展轴  0 = 轴0  1 = 轴1
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate int BSL_GetAxisCoorPulse([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis);

        //复位扩展轴坐标
        //输入参数:  szDevId=设备ID,axis扩展轴  0 = 轴0  1 = 轴1,bAll是否是全部清零
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ResetAxisPos([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis, bool bAll);

        //紧急停止
        //输入参数:  szDevId=设备ID
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_EmergenyStop([MarshalAs(UnmanagedType.LPWStr)] string szDevId);

        //获取扩展轴参数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetAxisParam(
            [MarshalAs(UnmanagedType.LPWStr)] string szDevId,           //设备ID
            [MarshalAs(UnmanagedType.LPWStr)] string szParName,         //参数名
            int axis,                       //扩展轴  0 = 轴0  1 = 轴1
            ref ushort ROTAXIS,           // 是否旋转轴 1-是，0-不是
            ref ushort REVROT,            // 是否反转	1-反转，0-正转
            ref int nAxisDir,          // 用户设置的，扩展轴方向：0-3分别表示X,Y,Z,U
            ref ushort nPPR,              // 每转脉冲数
            ref float LPR,               // 每转螺间距,mm
            ref float MINCOORD,          // 最小坐标,mm
            ref float MAXCOORD,          // 最大坐标,mm
            ref float MINPPS,            // 最小速度，脉冲/秒
            ref float MAXPPS,            // 最大速度，脉冲/秒
            ref float RUNPPS,            // 电机转速，脉冲/秒
            ref float STTPPS,            // 电机启动速度, 脉冲/s ，STTPPS 必须小于 RUNPPS
            ref float ACCTIME,           // 加速时间,ms

            ref ushort nZeroType,         // 零点类型，0：低电平、1：高电平、2：脉冲
            ref float fZeroPPS,          // 回0速度, 脉冲/s
            ref float fZeroTimeout,      // 回0超时，s
            ref ushort AccurToZero,       // 是否精确回零
            ref float PPDEC,             // 减速比, 是旋转轴时有效
            ref ushort nMoMode,           // 电机回零模式: 0-默认模式， 1-回零模式
            ref ushort nMoOrignLevel,     // 电机电平: 1-原点是高电平，0-原点是低电平

            ref float fOrgX,             //平台原点（零点）,用于分割与移动标刻，标刻过程中需使用
            ref float fOrgY,             //平台原点（零点）,用于分割与移动标刻，标刻过程中需使用
            ref bool bAutoZero,         //是否自动回零，true x,y同时回零，否则都不回零,回零时按bBackOrigin指定的方式回
            ref bool bBackOrigin,       //运行方式是否直回原点,true:直回原点，false:原路返回,标刻过程中需使用

            //扩展轴校正参数
            ref float DISLOCCOMPENSATE,  // 错位补偿, mm,当机械制造的误差比较大时，在平面拼图加工时会导致错位的现象，调节此参数可消除错位现象。
            ref float GAPCOMPENSATE,     // 间隙补偿(mm),补偿在运动时齿轮间的间隙误差

            ref float LIGHTOUTDEVIATION, // 出光时间误差(ms)
            ref float MARKSPEEDDEVIATION,// 打标速度偏差(ms/s)
            ref float ROTTIMEDEVIATION,  // 旋转时间偏差(ms/s)
            ref float POINTDISTENCE,     // 两点间距离阀值(mm)

            ref float GVSX,              //X桶形失真系数
            ref float GVSY,              //Y桶形失真系数
            ref float GVT                   //梯形失真系数
            );

        //显示扩展轴参数对话框
        //输入参数:  无
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate void BSL_ShowAxisCfgDlg([MarshalAs(UnmanagedType.LPWStr)] string szParName);


        #endregion

        #region 对象

        //清除对象库里所有数据
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ClearEntityLib([MarshalAs(UnmanagedType.LPWStr)] String strFileName);

        //对齐方式时数字代表的意义
        //   0 ---  1 --- 2
        //加入新文本到数据库中
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AddTextToFile(
        [MarshalAs(UnmanagedType.LPWStr)] String strFileName, // 文本增加到的目标文件名，增加后仍然需要手动保存才能保存到文件。
        [MarshalAs(UnmanagedType.LPWStr)] String pStr,   //要添加的字符串
        [MarshalAs(UnmanagedType.LPWStr)] String pEntName,  //字符串对象名称
        double dPosX,//字符串的左下角基点的x坐标
        double dPosY,//字符串的左下角基点的y坐标
        double dPosZ,//字符串对象的z坐标
        int nAlign,//对齐方式0－8
        double dTextRotateAngle,//字符串绕基点旋转的角度值(弧度值)
        int nPenNo,//对象使用的加工参数
        bool bHatchText);//是否填充文本对象

        //加入新文本到数据库中
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AddTextToFileEx(
        [MarshalAs(UnmanagedType.LPWStr)] String strFileName, // 文本增加到的目标文件名，增加后仍然需要手动保存才能保存到文件。
        [MarshalAs(UnmanagedType.LPWStr)] String pStr,   //要添加的字符串
        [MarshalAs(UnmanagedType.LPWStr)] String pEntName,  //字符串对象名称
        double dPosX,//字符串的左下角基点的x坐标
        double dPosY,//字符串的左下角基点的y坐标
        double dPosZ,//字符串对象的z坐标
        int nAlign,//对齐方式0－2
        double dTextRotateAngle,//字符串绕基点旋转的角度值(弧度值)
        int nPenNo,//对象使用的加工参数
        bool bHatchText,//是否填充文本对象
        double dHeight,  //字体高度
        [MarshalAs(UnmanagedType.LPWStr)] string pTextFontName);  //字体名称

        //加入指定文件到数据库中
        //支持的文件有dxf,plt,ai,svg等
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AddVectorToFile(
        [MarshalAs(UnmanagedType.LPWStr)] string strFileName,   //模板(orzx)文件名称
        [MarshalAs(UnmanagedType.LPWStr)] string sFileName,     //要添加的矢量文件名称(支持的文件格式有svg,dxf, plt, ai等)
        [MarshalAs(UnmanagedType.LPWStr)] string pEntName,      //字符串对象名称(为NULL则自动截取文件名 非NULL由外部传入)
        bool bHatchFile,             //是否填充文件对象 如果是ezd文件或位图文件此参数无效
        bool bCenter);               //加载后将矢量图居中

        //加入图片到数据库中
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AddBmpToFile(
        [MarshalAs(UnmanagedType.LPWStr)] string strFileName,   //模板(orzx)文件名称
        [MarshalAs(UnmanagedType.LPWStr)] string sBmpFileName,      //要添加图片的路径,支持gif,jpg,jpeg,png,bmp,tif.tiff,emf格式图片
        [MarshalAs(UnmanagedType.LPWStr)] string pEntName,      //字符串对象名称(为NULL则自动截取文件名 非NULL由外部传入)
        double W,                //尺寸X
        double H,                //尺寸Y
        double dPosX,            //中心点的x坐标
        double dPosY,            //中心点的y坐标
        double dRotateAngle,     //绕基点旋转的角度值(弧度值)
        int nPenNo               //对象使用的加工参数
        );

        //加入曲线到数据库中
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AddCurveToFile(
        IntPtr ptBuf,//曲线顶点数组
        int ptNum,//曲线顶点数
        [MarshalAs(UnmanagedType.LPWStr)] string pEntName,//曲线对象名称
        int nPenNo,//曲线对象使用的笔号
        int bHatch);//曲线是否填充

        //加入多直线段到数据库中
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AddLinesToFile(
        [MarshalAs(UnmanagedType.LPWStr)] string szFileName,//目标文件名
        IntPtr ptBuf,//直线段顶点数组
        int ptNum,//直线段顶点数
        [MarshalAs(UnmanagedType.LPWStr)] string pEntName,//直线段对象名称
        int nPenNo); //直线对象使用的笔号
                     //加入条码到数据库中
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AddBarcodeToFile(
        [MarshalAs(UnmanagedType.LPWStr)] string strFileName,
        [MarshalAs(UnmanagedType.LPWStr)] string pStr,   //要添加的字符串
        [MarshalAs(UnmanagedType.LPWStr)] string pEntName,  //字符串对象名称
        double dPosX, //字符左下角基点x坐标
        double dPosY, //字符左下角基点y坐标
        double dPosZ, //字符z坐标
        int nAlign,//对齐方式0－8
        int nPenNo,
        //  int bHatchText,
        //  int nBarcodeType,//条码类型
        //   UInt16 wBarCodeAttrib,//条码属性
        //   double dHeight,//整个条码的高	
        //   double dNarrowWidth,//最窄模块宽	
        //   double[] dBarWidthScale,//条宽比例	(与最窄模块宽相比)
        //  double[] dSpaceWidthScale,//空宽比例(与最窄模块宽相比)
        //   double dMidCharSpaceScale,//字符间隔比例(与最窄模块宽相比)
        //  double dQuietLeftScale,//条码左空白宽度比例(与最窄模块宽相比)
        //   double dQuietMidScale,//条码中空白宽度比例(与最窄模块宽相比)
        //  double dQuietRightScale,//条码右空白宽度比例(与最窄模块宽相比)	
        //   double dQuietTopScale,//条码上空白宽度比例(与最窄模块宽相比)
        //   double dQuietBottomScale,//条码下空白宽度比例(与最窄模块宽相比)						 
        int nRow,//二维码行数
        int nCol,//二维码列数
        int nCheckLevel,//pdf417错误纠正级别0-8
        int nSizeMode,//DataMatrix尺寸模式0-30
                      //   double dTextHeight,//人识别字符字体高度
                      //   double dTextWidth,//人识别字符字体宽度
                      //   double dTextOffsetX,//人识别字符X方向偏移
                      //   double dTextOffsetY,//人识别字符Y方向偏移
                      //   double dTextSpace,//人识别字符间距
                      //  double dDiameter,
        [MarshalAs(UnmanagedType.LPWStr)] string pTextFontName
        );

        //获取数据库里的指定文本对象的文本
        //输入参数: strFileName		图形文件名称
        //			strTextName     要更改内容的文本对象的名称
        //          strText      新的文本内容
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetTextByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strTextName, char[] arrText);

        //获取数据库里的文本类型名称
        //输入参数: strFileName		图形文件名称
        //			strTextName     文本对象的名称
        //          nFontType       字体类型
        //          arrFontName     字体名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetFontTypeNameByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strTextName, ref int nFontType, char[] arrFontName);

        //更改当前数据库里的指定文本对象的文本
        //输入参数: strFileName		图形文件名称
        //			strTextName     要更改内容的文本对象的名称
        //          strTextNew      新的文本内容
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ChangeTextByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strTextName, [MarshalAs(UnmanagedType.LPWStr)] string strTextNew);

        //更改当前数据库里的指定文本群组对象的单个文本
        //输入参数: strFileName  图形文件名称
        //			strTextName     要更改内容的文本群组对象的名称(最大群组)
        //			iElementIndex	要更改内容的单个文本在文本群组对象图元列表中的索引号
        //          strTextNew      新的文本内容
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ChangeTextInGroupByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strTextName, int iElementIndex, [MarshalAs(UnmanagedType.LPWStr)] string strTextNew);

        //更改当前数据库里的指定文本对象的文本
        //输入参数: strFileName		图形文件名称
        //			iShapeIndex     要更改内容的文本对象在图形列表中的索引号
        //          strTextNew      新的文本内容
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ChangeTextByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 iShapeIndex, [MarshalAs(UnmanagedType.LPWStr)] string strTextName);

        //删除当前数据库里的指定文本对象
        //输入参数: strFileName		图形文件名称
        //			strEntName      要删除对象的名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_DeleteEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName);

        //删除当前数据库里的全部对象
        //输入参数: strFileName		图形文件名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_DeleteAllEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName);

        //保存当前数据库里所有对象到指定图形文件里
        //输入参数: strFileName 图形文件名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SaveEntLibToFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName);


        //得到指定对象的最大最小坐标
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetEntSizeByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int nIndex, ref double dMinx, ref double dMiny, ref double dMaxx, ref double dMaxy, ref double dZ);

        //得到指定对象的最大最小坐标
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetEntSizeByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, ref double dMinx, ref double dMiny, ref double dMaxx, ref double dMaxy, ref double dZ);

        // 复制选定图元
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_CopyEntByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, [MarshalAs(UnmanagedType.LPWStr)] string pNewEntName);

        // 复制选定图元
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_CopyEntByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, [MarshalAs(UnmanagedType.LPWStr)] string pNewEntName);

        //移动指定对象相对坐标
        //iIndex 对象索引号
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MoveEntityRelByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, double dMovex, double dMovey);

        //移动指定对象相对坐标
        //strEntName 对象名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MoveEntityRelByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, double dMovex, double dMovey);

        //移动指定对象绝对坐标
        //iIndex 对象索引号
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MoveEntityAbsByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, double dPtx, double dPty);

        //移动指定对象绝对坐标
        //strEntName 对象名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MoveEntityAbsByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, double dPtx, double dPty);

        //缩放指定对象，缩放中心坐标(dCenx，dCeny)  dScaleX=X方向缩放比例  dScaleY=Y方向缩放比例
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ScaleEntityByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName,
        int iIndex,//对象索引号
        double dCenx,
        double dCeny,
        double dScaleX,
        double dScaleY);

        //缩放指定对象，缩放中心坐标(dCenx，dCeny)  dScaleX=X方向缩放比例  dScaleY=Y方向缩放比例
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ScaleEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName,
        [MarshalAs(UnmanagedType.LPWStr)] string strEntName,//对象名称
        double dCenx,
        double dCeny,
        double dScaleX,
        double dScaleY);

        //镜像指定对象，镜像中心坐标(dCenx，dCeny)  bMirrorX=TRUE X方向镜像  bMirrorY=TRUE Y方向镜像
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MirrorEntityByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName,
        int iIndex,//对象索引号
        double dCenx,
        double dCeny,
        bool bMirrorX,
        bool bMirrorY);

        //镜像指定对象，镜像中心坐标(dCenx，dCeny)  bMirrorX=TRUE X方向镜像  bMirrorY=TRUE Y方向镜像
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MirrorEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName,
        [MarshalAs(UnmanagedType.LPWStr)] string strEntName,//对象名称
        double dCenx,
        double dCeny,
        bool bMirrorX,
        bool bMirrorY);

        //旋转指定对象  
        //iIndex对象索引号
        //(dCenx，dCeny) 旋转中心坐标
        //dAngle=旋转角度(逆时针为正，单位为度)  
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_RotateEntityByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, double dCenx, double dCeny, double dAngle);

        //旋转指定对象  
        //pEntName 对象名称
        //(dCenx，dCeny) 旋转中心坐标
        //dAngle = 旋转角度(逆时针为正，单位为度)  
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_RotateEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, double dCenx, double dCeny, double dAngle);

        //旋转指定对象  
        //iIndex对象索引号
        //(dCenx，dCeny) 旋转中心坐标
        //dAngle=旋转角度(逆时针为正，单位为度)  
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SlopeEntityByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, double dCenx, double dCeny, double dx, double dy);

        //旋转指定对象  
        //pEntName 对象名称
        //(dCenx，dCeny) 旋转中心坐标
        //dAngle = 旋转角度(逆时针为正，单位为度)  
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SlopeEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, double dCenx, double dCeny, double dx, double dy);

        //得到对象总数
        //输出参数:  对象总数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetEntityCount([MarshalAs(UnmanagedType.LPWStr)] string strFileName);

        //得到指定序号的对象名称
        //输入参数: nEntityIndex 指定对象的序号(围: 0 － (lmc1_GetEntityCount()-1))
        //输出参数: szEntName 对象的名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetEntityNameByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 nEntityIndex, [Out] ShapeName[] strEntName);

        //设置指定序号的实体名
        //输入参数: nEntityIndex 指定对象的序号(围: 0 － (lmc1_GetEntityCount()-1))
        //输出参数: szEntName 对象的名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SetEntityNameByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 nEntityIndex, [MarshalAs(UnmanagedType.LPWStr)] string strEntName);

        //设置指定序号的实体名
        //输入参数: nEntityIndex 指定对象的序号(围: 0 － (lmc1_GetEntityCount()-1))
        //输出参数: szEntName 对象的名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ChangeEntName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)] StringBuilder szOldEntName, [MarshalAs(UnmanagedType.LPWStr, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)] StringBuilder szNewEntName);

        // 移动图元位置
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MoveEntityOrderByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 iIndex, Int32 iOrderOffset);

        // 反向图元位置
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ReverseAllEntOrder([MarshalAs(UnmanagedType.LPWStr)] string strFileName);

        //填充对象
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_FillEntity([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, [MarshalAs(UnmanagedType.LPWStr)] string strEntNameNew);

        //删除填充
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_UnFillEnt([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, bool bUnGroup);

        // 设置填充参数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SetFillParam(ref BSL_FillPara fillPar);

        // 获取图元填充参数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetEntFillParam([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, ref BSL_FillPara fillPar);

        //屏蔽一块区域不标刻
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MaskArea([MarshalAs(UnmanagedType.LPWStr)] string strFileName, POINTF[] vPoints, int iPtCount);
        //全景SDK:

        //图元骨架信息,by tgf 20190129
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct EntityInfo
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
            public byte[] wszDocname;//文档名称，图元所属文档
            public int iIndex;  //在内存库中位置索引
            public int iType;       //图元类型
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
            public byte[] wszName;//图元名称
                                  //图元外接矩形的图纸坐标
            public double x;        //图元左顶点x坐标（mm）
            public double y;        //图元左顶点y坐标（mm）
            public double width;    //图元宽度（mm）
            public double height;   //图元高度（mm）
        };

        //图元骨架信息,by tgf 20190129
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct EntityInfoCSharp
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
            public byte[] wszDocname;//文档名称，图元所属文档
            public int iIndex;  //在内存库中位置索引
            public int iType;       //图元类型
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
            public byte[] wszName;//图元名称
                                  //图元外接矩形的图纸坐标
            public double x;        //图元左顶点x坐标（mm）
            public double y;        //图元左顶点y坐标（mm）
            public double width;    //图元宽度（mm）
            public double height;   //图元高度（mm）
        };

        //区域分组图元信息,by tgf 20190129
        //分组不能跨越文档
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct AreaEntityGroupCSharp
        {
            //区域范围
            public double x;        //图元左顶点x坐标（mm）
            public double y;        //图元左顶点y坐标（mm）
            public double width;    //图元宽度（mm）
            public double height;   //图元高度（mm）

            public int nCount;      //分组内图元数量
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
            public EntityInfo[] pEntity;//分组内图元
        };

        //区域分组图元信息,by tgf 20190129
        //分组不能跨越文档
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct BlockPosPathCShape
        {
            //区域范围
            public double x;        //图元左顶点x坐标（mm）
            public double y;        //图元左顶点y坐标（mm）
            public double width;    //图元宽度（mm）
            public double height;   //图元高度（mm）
        };

        //获取缓存中的全部图元骨架信息 for C#
        //输出参数: vEntities 图元骨架信息容器
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetAllEntities2([MarshalAs(UnmanagedType.LPWStr)] string szDocName, ref int nCount, [Out] EntityInfoCSharp[] pEntities);

        //获取缓存中的外接矩形小于给定尺寸的图形，并且按区域全部图元骨架信息   for C#
        //输入参数：width - 宽度 mm
        //			height - 高度 mm
        //输出参数: vEntities 图元骨架信息容器
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetEntitiesBySize2([MarshalAs(UnmanagedType.LPWStr)] string szDocName, double width, double height, ref int nCount, [Out] AreaEntityGroupCSharp[] pGroup);

        //标刻一个分组内的图元   for C#
        //by tgf 20190129
        //输入参数：szDevId 设备ID
        //          group 图元骨架信息容器
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MarkEntitiesInGroup2([MarshalAs(UnmanagedType.LPWStr)] string szDevId, AreaEntityGroupCSharp groupC);

        //修改当前数据库中的手工校正参数
        //输入参数：szParName  将要设置的参数名称
        //输入参数: dScaleX  X轴方向的放缩比例
        //输入参数: dScaleY  Y轴方向的放缩比例
        //输入参数: dDistorX  X轴方向的桶形失真系数
        //输入参数: dDistorY  Y轴方向的桶形失真系数
        //输入参数: dHorverX  X轴方向的平行四边形失真系数
        //输入参数: dHorverY  Y轴方向的平行四边形失真系数
        //输入参数: dTrapedistorX  X轴方向的梯形失真系数
        //输入参数: dTrapedistorY  Y轴方向的梯形失真系数
        //输入参数: bSaveToFile  是否保存到标准配置文件(BslCAD.cfg)
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ModifyManualCorPara([MarshalAs(UnmanagedType.LPWStr)] string szParName,
        double dScaleX,
        double dScaleY,
        double dDistorX,
        double dDistorY,
        double dHorverX,
        double dHorverY,
        double dTrapedistorX,
        double dTrapedistorY,
        bool bSaveToFile);

        //填充单个对象
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_FillSingleEntity([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int nShapeIndex, [MarshalAs(UnmanagedType.LPWStr)] string pEntNameNew);
        //删除单个填充
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_UnFillSingleEnt([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int nShapeIndex, bool bUnGroup);

        //GCODE
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STU_GCODE
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
            public byte[] wszGCode;
        };

        //导出G-CODE for C#
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ExportGCode([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [Out] STU_GCODE[] wstrGCode, ref int nCount);

        //导出G-CODE  by name for C#
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ExportGCodeByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, [Out] STU_GCODE[] wstrGCode, ref int nCount);

        //导出G-CODE by index for C#
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ExportGCodeByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, [Out] STU_GCODE[] wstrGCode, ref int nCount);

        //导出G-CODE by group for C#
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ExportGCodeByGroup2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, AreaEntityGroupCSharp groupC, [Out] STU_GCODE[] wstrGCode, ref int nCount);

        ////打标过程中设置旋转角度（度），偏移（mm），旋转中心（mm）
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate void BSL_SetOffsetValues(
        double dAngle,      //旋转角度(度)
        double dOffsetX,    //动态偏移x(mm)
        double dOffsetY,    //动态偏移y(mm)
        double dCenterX,    //旋转中心x(mm)
        double dCenterY);   //旋转中心y(mm)

        //设置设备关联的参数 旋转角度（度），偏移（mm），旋转中心（mm）
        //输入参数： szDevId    设备ID
        //输入参数： szFileName 模板文件
        //输入参数： szParName  设置的参数名称
        //输入参数： dAngle   旋转角度
        //输入参数： dOffsetX  X偏移
        //输入参数： dOffsetY  Y偏移
        //输入参数： dCenterX  旋转中心X坐标
        //输入参数： dCenterY  旋转中心Y坐标
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate void BSL_SetOffsetValuesOfDevice(
        [MarshalAs(UnmanagedType.LPWStr)] string szDevId,
        [MarshalAs(UnmanagedType.LPWStr)] string szFileName,
        [MarshalAs(UnmanagedType.LPWStr)] string szParName,
        double dAngle,
        double dOffsetX,
        double dOffsetY,
        double dCenterX,
        double dCenterY);

        /*
         * 根据传入的分块中心位置找到对应的块并打标
         * szDocName: 文件名
         * type ：分块类型 0为根据长宽分块
         * x：x反向分块长或者X方向分块数
         * y：y方向分块宽或者Y方向分块数
         * centralPoint：分块中心点位置
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_FindBlockMark([MarshalAs(UnmanagedType.LPWStr)] string szDocName, int type, double x, double y, POINTF centralPoint);
        //输入参数: strFileName		图形文件名称
        //			strTextName     要更改内容的文本对象的名称
        //          strTextNew      新的文本内容
        //[UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        //public delegate BslErrCode BSL_ChangeTextByName([MarshalAs(UnmanagedType.LPWStr)] string szDocName, [MarshalAs(UnmanagedType.LPWStr)] string strTextName, [MarshalAs(UnmanagedType.LPWStr)] string strTextNew);

        /** 图元对象是否为变量文本
          * 输入参数： szFileName --图形文件名称
          * 输入参数： pEntName --图元对象名称
          * 输出参数： bVarText --true,表示变量文本
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_IsVarText([MarshalAs(UnmanagedType.LPWStr)] string szFileName, [MarshalAs(UnmanagedType.LPWStr)] string pEntName, ref bool bVarText);

        //重置序列号
        //输入参数: strFileName		图形文件名称
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ResetSN([MarshalAs(UnmanagedType.LPWStr)] string strFileName);

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetLastMarkTextByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntity, char[] arrLaserText);

        //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        //public struct BLS_TextFontParam
        //{
        //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        //    public byte[] wszTextName;
        //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        //    public byte[] wszFontName;
        //    public double dCharHeight;
        //    public double dCharWidth;
        //    public double dCharAngle;
        //    public double dCharSpace;
        //    public double dLineSpace;
        //    public bool bEqualCharWidth;
        //};
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SetTextEntParam(
       [MarshalAs(UnmanagedType.LPWStr)] string strFileName,
       [MarshalAs(UnmanagedType.LPWStr)] string strEntity,
       StringBuilder strFontName,
       double dCharHeight,
       double dCharWidth,
       double dCharAngle,
       double dCharSpace,
       double dLineSpace,
       bool bEqualCharWidth
       );

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetTextEntParam(
        [MarshalAs(UnmanagedType.LPWStr)] string strFileName,
        [MarshalAs(UnmanagedType.LPWStr)] string strEntity,
        StringBuilder strFontName,
        ref double dCharHeight,
        ref double dCharWidth,
        ref double dCharAngle,
        ref double dCharSpace,
        ref double dLineSpace,
        ref bool bEqualCharWidth
        );
        //加入新圆到数据库中
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AddCircleToFile(
         [MarshalAs(UnmanagedType.LPWStr)] string szFileName,//圆增加到的目标文件名，增加后仍然需要手动保存才能保存到文件。
         [MarshalAs(UnmanagedType.LPWStr)] string pEntName,//圆对象名称
         double dPosX,//圆中心点的x坐标
         double dPosY,//圆中心点的y坐标
         double dPosZ,//圆对象的z坐标
         double dRadius,//圆半径
         double dRotateAngle,//绕中心点旋转的角度值(弧度值)
         int nPenNo,//对象使用的加工参数
         bool bFill //是否填充对象
         );
        //加入新椭圆到数据库中
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_AddEllipseToFile(
        [MarshalAs(UnmanagedType.LPWStr)] string szFileName,//圆增加到的目标文件名，增加后仍然需要手动保存才能保存到文件。
        [MarshalAs(UnmanagedType.LPWStr)] string pEntName,//圆对象名称
        double dPosX,//圆中心点的x坐标
        double dPosY,//圆中心点的y坐标
        double dPosZ,//圆对象的z坐标
        double dLongAxis,//椭圆长轴
        double dMinorAxis,//椭圆短轴
        double dRotateAngle,//绕中心点旋转的角度值(弧度值)
        int nPenNo,//对象使用的加工参数
        bool bFill //是否填充对象
        );

        /*///////////////////////////////
         _________________  
        |		 |		  | X1
        |		 |		  |
        |		 | a 	  |	
        |--------|--------| X2
        |        |		  |
        |		 |		  |
        |________|________| X3
        Y1       Y2      Y3
        /*///////////////////////////////
           //根据当前数据库中手工校正参数和实际测量数据计算产生推荐值
           //输入参数: dx_T  X轴方向理论边长 mm
           //输入参数: dy_T  Y轴方向理论边长 mm
           //输入参数: da_A  角度a的实际测量角度(°)
           //输入参数: dx1_A  实际测量的X1的长度 mm
           //输入参数: dx2_A  实际测量的X2的长度 mm
           //输入参数: dx3_A  实际测量的X3的长度 mm
           //输入参数: dy1_A  实际测量的Y1的长度 mm
           //输入参数: dy2_A  实际测量的Y2的长度 mm
           //输入参数: dy3_A  实际测量的Y3的长度 mm
           //输出参数: dScaleX  X轴方向的放缩比例
           //输出参数: dScaleY  Y轴方向的放缩比例
           //输出参数: dDistorX  X轴方向的桶形失真系数
           //输出参数: dDistorY  Y轴方向的桶形失真系数
           //输出参数: dHorverX  X轴方向的平行四边形失真系数
           //输出参数: dHorverY  Y轴方向的平行四边形失真系数
           //输出参数: dTrapedistorX  X轴方向的梯形失真系数
           //输出参数: dTrapedistorY  Y轴方向的梯形失真系数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GenerateRecommendedValues(
        double dx_T,
        double dy_T,
        double da_A,
        double dx1_A,
        double dx2_A,
        double dx3_A,
        double dy1_A,
        double dy2_A,
        double dy3_A,
        ref double dScaleX,
        ref double dScaleY,
        ref double dDistorX,
        ref double dDistorY,
        ref double dHorverX,
        ref double dHorverY,
        ref double dTrapedistorX,
        ref double dTrapedistorY
        );
        #endregion

        #region win  API
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary (string lpFileName);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress (IntPtr lib, string funcName);
        [DllImport("kernel32.dll")]
        private static extern IntPtr FreeLibrary (IntPtr lib);
        [DllImport("user32.dll", EntryPoint = "EnumWindows", SetLastError = true)]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);

        [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

        [DllImport("user32.dll", EntryPoint = "IsWindow")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(uint dwErrCode);
        #endregion

        private static Hashtable processWnd = null;

        public delegate bool WNDENUMPROC(IntPtr hwnd, uint IParam);

        static MarkAPI() { 
            if (processWnd == null ) 
            {
                processWnd = new Hashtable ();  
            }
        }

        public IntPtr hLib;
        public MarkAPI(string DllPath)
        {
            hLib = LoadLibrary(DllPath);
        }

        ~MarkAPI()
        {
            
        }

        public Delegate Invoke(string APIName, Type t) 
        {
            IntPtr api = GetProcAddress (hLib, APIName);
            if (api == IntPtr.Zero) 
            {
                Console.WriteLine("加载api" + APIName + "失败");
                Console.WriteLine("***************************************************************************");
                return null;
            }
            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }
        /// <summary>
        /// 返回控制台句柄（Handle）
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetCurrentWindowHandle()
        {
            IntPtr ptrWnd = IntPtr.Zero;
            uint uiPid = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;  // 当前进程 ID
            object objWnd = processWnd[uiPid];

            if (objWnd != null)
            {
                ptrWnd = (IntPtr)objWnd;
                if (ptrWnd != IntPtr.Zero && IsWindow(ptrWnd))  // 从缓存中获取句柄
                {
                    return ptrWnd;
                }
                else
                {
                    ptrWnd = IntPtr.Zero;
                }
            }

            bool bResult = EnumWindows(new WNDENUMPROC(EnumWindowsProc), uiPid);
            // 枚举窗口返回 false 并且没有错误号时表明获取成功
            if (!bResult && Marshal.GetLastWin32Error() == 0)
            {
                objWnd = processWnd[uiPid];
                if (objWnd != null)
                {
                    ptrWnd = (IntPtr)objWnd;
                }
            }

            return ptrWnd;
        }

        private static bool EnumWindowsProc(IntPtr hwnd, uint lParam)
        {
            uint uiPid = 0;

            if (GetParent(hwnd) == IntPtr.Zero)
            {
                GetWindowThreadProcessId(hwnd, ref uiPid);
                if (uiPid == lParam)    // 找到进程对应的主窗口句柄
                {
                    processWnd[uiPid] = hwnd;   // 把句柄缓存起来
                    SetLastError(0);    // 设置无错误
                    return false;   // 返回 false 以终止枚举窗口
                }
            }

            return true;
        }
    }
}
