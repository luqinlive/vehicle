using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Xml;

namespace TIEVision.COMMON
{

    public class HOBJ
    {
        #region   初始化

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CvRect_t
        {
            public int x;
            public int y;
            public int width;
            public int height;
        }

        public struct SObjFeat
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 180)]
            public double[] AppFeatData;			//匹配结果信息
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            public double[] ColorFeatData;			//匹配结果信息
        };//(180+28)*8 = 1664

        public struct SObj
        {
            public int size;				//结构大小(作为版本号来处理)
            public int ver;				//1.0

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string road;			//过车路口
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
            public string date;			//过车时间

            public int objtype;		//类型	//0-行人;1-自行车;2-摩托车;3-电动摩托车;4-三轮车;5-小型车;6-大车;7-开车;8-拖拉机;9-中巴
            public int objcolor;	//颜色

            public CvRect_t rect;	//位置
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string filename;			//文件名称

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] reserved;			// 保留
        };//4*8 = 32+256 = 288

        public struct SObjEx
        {
            public int size;				//结构大小(作为版本号来处理)
            public int ver;				//1.0

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string road;			//过车路口
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
            public string date;			//过车时间

            public int objtype;		//类型	//0-行人;1-自行车;2-摩托车;3-电动摩托车;4-三轮车;5-小型车;6-大车;7-开车;8-拖拉机;9-中巴
            public int objcolor;	//颜色

            public CvRect_t rect;	//位置
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string filename;			//文件名称

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            //public byte[] reserved;			// 保留
        };//4*8 = 32+256 = 288

        [Serializable]
        public struct HOBJInfo
        {
            public SObj info;
            public SObjFeat feat;	//特征
        };//1664+288= 1952

        public struct SObjQuery
        {
            public int taskid;			    //                
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string time_begin;//yyyyMMdd;			//开始时间                    
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string time_end;//yyyyMMdd;			    //结束时间  

            public int sel_road;	//选择条件 地点
            public int sel_date;	//选择条件 时间
            public int sel_plate;	//选择条件 车牌
            public int sel_color;	//选择条件 颜色
            public int sel_type;	//选择条件 车型
            public int sel_head;	//选择条件 车头车尾

            public SObj info;
        };

        public struct SObjQueryParam
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string code;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string uuid;


            public HOBJInfo info;
            public SObjQuery query;
        }


        public struct HOBJResult
        {
            public int id;						//文件标识
            public int confi;					//置信度----综合
            public int confi_type;				//置信度----类型
            public int confi_color;				//置信度----颜色

            public SObj info;

 //           public SObjEx info;
        };

        #endregion   初始化
    }
}
