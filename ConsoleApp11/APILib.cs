using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp11
{
    public class APILib
    {

        const string DLL_NAME = "nano_api.dll";
        const string DLL_NAME2 = "libdlpspec.dll"; 

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_ApplyScanConfig(IntPtr pBuffer, int bufSize);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_SetActiveScanIndex(byte index);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_SetScanNumRepeats(UInt16 num);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_GetEstimatedScanTime();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_PerformScan(bool StoreInSDCard);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_GetScanComplete();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_GetFileSizeToRead(NNO_FILE_TYPE fileType);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_GetFileData(IntPtr pData, ref int pSizeInBytes);
        [DllImport(DLL_NAME2, CallingConvention = CallingConvention.Cdecl)]
        public static extern DLPSPEC_ERR_CODE dlpspec_scan_write_configuration(ref uScanConfig pCfg, IntPtr pBuf, UIntPtr bufSize);

        [DllImport(DLL_NAME2, CallingConvention = CallingConvention.Cdecl)]
        public static extern DLPSPEC_ERR_CODE dlpspec_scan_interpret(IntPtr pBuf, UIntPtr bufSize, ref scanResults pResults);

    }
    public enum NNO_FILE_TYPE
    {
        NNO_FILE_SCAN_DATA,
        NNO_FILE_SCAN_CONFIG,
        NNO_FILE_REF_CAL_DATA,
        NNO_FILE_REF_CAL_MATRIX,
        NNO_FILE_HADSNR_DATA,
        NNO_FILE_SCAN_CONFIG_LIST,
        NNO_FILE_SCAN_LIST,
        NNO_FILE_SCAN_DATA_FROM_SD,
        NNO_FILE_INTERPRET_DATA,
        NNO_FILE_MAX_TYPES
    }
    public enum DLPSPEC_ERR_CODE
    {
        // add the enum values here. I don't see the actual values in the header file you provided
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct uScanConfig
    {
        // add the fields here. You'll need to create other structs for scanConfig and slewScanConfig
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct scanResults
    {
        // add the fields here
    }
}
