using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TIEVision.Model;

namespace TIEVision.COMMON
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TyCalcObj
    {
        public IntPtr pszData;
        public int nLen;
        public int nW;
        public int nH;
        public int nFmt;
        public int nType;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TyCalcRelt
    {
        public int nNums;
        public IntPtr pCalcObj;
    }

    public class HNodeLib
    {
        
        public delegate void ShowTargetResultEventHandler(TargetInfoReslt resultInfos);
        public static event ShowTargetResultEventHandler ShowTargetResultInfo;

        public delegate void ShowProgressEventHandler(string pszId, int nSenconds);
        public static event ShowProgressEventHandler ShowProgress;

        const string libraryName = "TmNodeLib.dll";

        public delegate bool  HNode_ResultProc([MarshalAs(UnmanagedType.LPStr)]string pszID, [MarshalAs(UnmanagedType.LPStr)]string pszJson, IntPtr relt, IntPtr pThis );

        public static HNode_ResultProc proc = new HNode_ResultProc(HNode_Result);

        public delegate bool TyProcessCbFunc([MarshalAs(UnmanagedType.LPStr)]string pszID, int nSeconds, IntPtr pThis);
        public static TyProcessCbFunc processCbFunc = new TyProcessCbFunc(CbProcess_Result);

        /*
         * 结果回调
         * */
        [DllImport(libraryName, EntryPoint = "SetResultCb", CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetResultCb(HNode_ResultProc callback, IntPtr pThis);
        /*
         * 进度回调
         * */
        [DllImport(libraryName, EntryPoint = "SetProcessCb", CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetProcessCb(TyProcessCbFunc callback, IntPtr pThis);
        /*
         * 初始化
         * */
        [DllImport(libraryName, EntryPoint = "InitNodeLib", CallingConvention = CallingConvention.StdCall)]
        private static extern bool InitNodeLib();


        [DllImport(libraryName, EntryPoint = "LoadConfig", CallingConvention = CallingConvention.StdCall)]
        private static extern bool LoadConfig([MarshalAs(UnmanagedType.LPStr)]string pszXml);

        [DllImport(libraryName, EntryPoint = "AddGroup", CallingConvention = CallingConvention.StdCall)]
        private static extern bool AddGroup([MarshalAs(UnmanagedType.LPStr)]string pszParentID,[MarshalAs(UnmanagedType.LPStr)]string pszXml);

        [DllImport(libraryName, EntryPoint = "AddDevice", CallingConvention = CallingConvention.StdCall)]
        private static extern bool AddDevice([MarshalAs(UnmanagedType.LPStr)]string pszParentID, [MarshalAs(UnmanagedType.LPStr)]string pszXml);

        [DllImport(libraryName, EntryPoint = "AddFile", CallingConvention = CallingConvention.StdCall)]
        private static extern bool AddFile([MarshalAs(UnmanagedType.LPStr)]string pszParentID, [MarshalAs(UnmanagedType.LPStr)]string pszXml);

        [DllImport(libraryName, EntryPoint = "ModFile", CallingConvention = CallingConvention.StdCall)]
        private static extern bool ModFile([MarshalAs(UnmanagedType.LPStr)]string pszParentID, [MarshalAs(UnmanagedType.LPStr)]string pszXml);

        [DllImport(libraryName, EntryPoint = "ModDevice", CallingConvention = CallingConvention.StdCall)]
        private static extern bool ModDevice([MarshalAs(UnmanagedType.LPStr)]string pszParentID, [MarshalAs(UnmanagedType.LPStr)]string pszXml);


        [DllImport(libraryName, EntryPoint = "SetVideoCb", CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetVideoCb(IntPtr callback, IntPtr pHwnd, IntPtr pThis);

        [DllImport(libraryName, EntryPoint = "VideoPlay", CallingConvention = CallingConvention.StdCall)]
        private static extern bool VideoPlay([MarshalAs(UnmanagedType.LPStr)]string pszID, int nTimes, bool bCalc);

        [DllImport(libraryName, EntryPoint = "SetPlayWin", CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetPlayWin([MarshalAs(UnmanagedType.LPStr)]string pszID, IntPtr pHwnd);

        [DllImport(libraryName, EntryPoint = "VideoStop", CallingConvention = CallingConvention.StdCall)]
        private static extern bool VideoStop([MarshalAs(UnmanagedType.LPStr)]string pszID);


        [DllImport(libraryName, EntryPoint = "VideoPause", CallingConvention = CallingConvention.StdCall)]
        private static extern bool VideoPause([MarshalAs(UnmanagedType.LPStr)]string pszID);

        [DllImport(libraryName, EntryPoint = "VideoNextFrm", CallingConvention = CallingConvention.StdCall)]
        private static extern bool VideoNextFrm([MarshalAs(UnmanagedType.LPStr)]string pszID);

        [DllImport(libraryName, EntryPoint = "VideoPreFrm", CallingConvention = CallingConvention.StdCall)]
        private static extern bool VideoPreFrm([MarshalAs(UnmanagedType.LPStr)]string pszID);

        [DllImport(libraryName, EntryPoint = "VideoPos", CallingConvention = CallingConvention.StdCall)]
        private static extern bool VideoPos([MarshalAs(UnmanagedType.LPStr)]string pszID, int nOffsetFmt, bool  bFlag);

        [DllImport(libraryName, EntryPoint = "VideoInfo", CallingConvention = CallingConvention.StdCall)]
        private static extern bool VideoInfo([MarshalAs(UnmanagedType.LPStr)]string pszID,ref int nFrmRate, ref int nFrms,ref long nDuration,ref long nSize);

        [DllImport(libraryName, EntryPoint = "UninitNodeLib", CallingConvention = CallingConvention.StdCall)]
        private static extern bool UninitNodeLib();


        public static bool InitVideoLib()
        {
            bool bRet = InitNodeLib();
            bool ret = SetResultCb(proc, IntPtr.Zero);
            bRet= SetProcessCb(processCbFunc, IntPtr.Zero);
            return bRet;
        }

        public static bool HNode_Result(string pszID, string pszJson, IntPtr relt, IntPtr pThis )
        {
            //return true;
            try
            {
                TargetInfoReslt resultInfo = JsonConvert.DeserializeObject<TargetInfoReslt>(pszJson);
                if(resultInfo.CalcType =="4")
                {
                    resultInfo.FaceImages = new List<Image>();
                }
                //byte[] bytes = new byte[size];
                //Marshal.Copy(buf, bytes, 0, size);

                TyCalcRelt tyCalcRelt = new TyCalcRelt();
                int size_struct = Marshal.SizeOf(tyCalcRelt);
                //分配结构体内存空间
                //IntPtr structPtr = Marshal.AllocHGlobal(size_struct);
                //将byte数组拷贝到分配好的内存空间
                //Marshal.Copy(bytes, 0, structPtr, size_struct);
                //将内存空间转换为目标结构体
                tyCalcRelt = (TyCalcRelt)Marshal.PtrToStructure(relt, typeof(TyCalcRelt));

                TyCalcObj[] tyCalcObj = new TyCalcObj[tyCalcRelt.nNums];
                int size_obj = Marshal.SizeOf(typeof(TyCalcObj));

                for (int i = 0; i < tyCalcRelt.nNums; i++)
                {
                    IntPtr p = new IntPtr((tyCalcRelt.pCalcObj.ToInt64() + i * size_obj));
                    tyCalcObj[i] = (TyCalcObj)Marshal.PtrToStructure(p, typeof(TyCalcObj));
                    byte[] BytesKeyImage = new byte[tyCalcObj[i].nLen];
                    if (tyCalcObj[i].pszData != IntPtr.Zero)
                    {
                        Marshal.Copy(tyCalcObj[i].pszData, BytesKeyImage, 0, tyCalcObj[i].nLen);
                        
                        Image img = BytesToImage(BytesKeyImage);
                        //img.Save("D:\\fsdfd" + i + ".jpg", ImageFormat.Jpeg);

                        int nTypes = tyCalcObj[i].nType;
                        if (resultInfo.CalcType == "4")
                        {
                            if (nTypes == 0)
                            {
                                resultInfo.FaceImages.Add(img);
                            }
                            else if (nTypes == 1)
                            {
                                resultInfo.BackgroundImage = img;
                            }
                        }
                        else if(resultInfo.CalcType == "8")
                        {
                            if (nTypes == 0)
                            {
                                resultInfo.KeyImage = img;
                            }
                            else if (nTypes == 1)
                            {
                                resultInfo.BackgroundImage = img;
                            }
                        }
                        
                        
                    }

                }

                try
                {
                    ShowTargetResultInfo(resultInfo);
                }
                catch
                {

                }
                //ZmqTargetResult.GetInstance().SendTargetResult(pszJson);
                //释放内存空间
                //Marshal.FreeHGlobal(structPtr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            return true;
        }

        public static bool CbProcess_Result(string pszID, int nSeconds, IntPtr pThis)
        {
            //Console.WriteLine("process:"+pszID + nSeconds);
            try
            {
                if (null != ShowProgress)
                    ShowProgress(pszID, nSeconds);
            }
            catch { }
            return true;
        }
        public static Image BytesToImage(byte[] buffer)
        {
            try
            {
                MemoryStream ms = new MemoryStream(buffer);
                Image image = System.Drawing.Image.FromStream(ms);
                return image;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static bool LoadAcqConfig(string xmlPath)
        {
            return LoadConfig(xmlPath);
        }

        public static bool AddGroupXml(string pszParentID, string pszXml)
        {
            return AddGroup(pszParentID, pszXml);
        }
        public static bool AddDeviceXml(string pszParentID, string pszXml)
        {
            return AddDevice(pszParentID, pszXml);
        }
        public static bool AddFileXml(string pszParentID, string pszXml)
        {
            return AddFile(pszParentID, pszXml);
        }

        public static bool ModFileXml(string pszParentID, string pszXml)
        {
            return ModFile(pszParentID, pszXml);
        }

        public static bool ModDeviceXml(string pszParentID, string pszXml)
        {
            return ModDevice(pszParentID, pszXml);
        }

        static int n = 10;
        public static bool SetVideoPlay( IntPtr pHwnd)
        {

            IntPtr ptrCallback = (IntPtr) 0xffffffff;
            

            return SetVideoCb(ptrCallback, pHwnd, IntPtr.Zero);
        }

        public static bool UninitVideoLib()
        {
            return UninitNodeLib();
        }

        public static bool VideoPlayControl(string videId, int nTimes)
        {
            
            return VideoPlay(videId, nTimes, true);
        }

        public static bool SetPlayWinControl(string videid, IntPtr pHwnd)
        {
            return SetPlayWin(videid, pHwnd);
        }

        public static bool VideoPlayControlStop(string videId)
        {
            return VideoStop(videId );
        }

        public static bool VideoPlayControlPause(string videId)
        {
            return VideoPause(videId);
        }

        public static bool VideoPlayControlNextFrame(string videId)
        {
            return VideoNextFrm(videId);
        }

        public static bool VideoPlayControlPrevsFrame(string videId)
        {
            return VideoPreFrm(videId);
        }

        public static bool VideoPlayControlVideoPos(string videId, int keyIndex, bool bFlag)
        {
            return VideoPos(videId, keyIndex, bFlag);
        }
        public static bool VideoInfoControl(string videId, ref int nFrmRate, ref int nFrms,ref  long nDuration,ref  long nSize)
        {
            return VideoInfo(videId,  ref nFrmRate,ref nFrms,ref nDuration,ref nSize);
        }

    }
}
