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
        #region API Funcitons
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_ApplyScanConfig(byte[] pBuffer, int bufSize);

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
        public static extern int NNO_SetHibernate(bool hibernate);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_SetUARTConnected(bool connected);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_GetFileData(IntPtr pData, ref int pSizeInBytes);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NNO_GetSerialNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder serial_number);
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int C_ApplyScanCfgtoDevice();
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int C_GetScanData(ref scanResults scan_results);
        #endregion
        #region USB Funcitons
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int USB_Init();
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int USB_Open();
        
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int USB_Close();
        
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool USB_IsConnected();
        #endregion
        #region Serial Port functions
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Serial_Open(string Port);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Serial_Close();

        #endregion
        #region DLPSPEC Funcitons
        [DllImport(DLL_NAME2, CallingConvention = CallingConvention.Cdecl)]
        public static extern DLPSPEC_ERR_CODE dlpspec_get_scan_config_dump_size(ref uScanConfig pCfg, out UIntPtr pBufSize);
        
        [DllImport(DLL_NAME2, CallingConvention = CallingConvention.Cdecl)]
        public static extern DLPSPEC_ERR_CODE dlpspec_scan_write_configuration(ref uScanConfig pCfg, IntPtr pBuf, UIntPtr bufSize);

        [DllImport(DLL_NAME2, CallingConvention = CallingConvention.Cdecl)]
        public static extern DLPSPEC_ERR_CODE dlpspec_scan_interpret(IntPtr pBuf, UIntPtr bufSize, ref scanResults pResults);
        #endregion
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
        DLPSPEC_PASS = 0,
        ERR_DLPSPEC_FAIL = -1,
        ERR_DLPSPEC_INVALID_INPUT = -2,
        ERR_DLPSPEC_INSUFFICIENT_MEM = -3,
        ERR_DLPSPEC_TPL = -4,
        ERR_DLPSPEC_ILLEGAL_SCAN_TYPE = -5,
        ERR_DLPSPEC_NULL_POINTER = -6
    }
    public enum ExpTime
    {
        T_635_US,
        T_1270_US,
        T_2450_US,
        T_5080_US,
        T_15240_US,
        T_30480_US,
        T_60960_US,
    }

    public struct SlewScanSection
    {
        public byte SectionScanType;
        public byte WidthPx;
        public ushort WavelengthStartNm;
        public ushort WavelengthEndNm;
        public ushort NumPatterns;
        public ExpTime ExposureTime;
    }

    public struct SlewScanConfig
    {
        public byte ScanType;
        public ushort ScanConfigIndex;
        public string ScanConfigSerialNumber;
        public string ConfigName;
        public ushort NumRepeats;
        public byte NumSections;
        public SlewScanSection Section1;
        public SlewScanSection Section2;
        public SlewScanSection Section3;
        public SlewScanSection Section4;
        public SlewScanSection Section5;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ScanConfig
    {
        public byte ScanType;
        public ushort ScanConfigIndex;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string ScanConfigSerialNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string ConfigName;
        public ushort WavelengthStartNm;
        public ushort WavelengthEndNm;
        public byte WidthPx;
        public ushort NumPatterns;
        public ushort NumRepeats;
    }


    public struct DateTimeStruct
    {
        public byte Year;
        public byte Month;
        public byte Day;
        public byte DayOfWeek;
        public byte Hour;
        public byte Minute;
        public byte Second;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ScanDataHeadBody
    {
        public short systemTempHundredths;
        public short detectorTempHundredths;
        public ushort humidityHundredths;
        public ushort lampPd;
        public uint scanDataIndex;
        // Assuming calibCoeffs is an array of doubles
        public double[] calibCoeffs;
        public string serialNumber;
        public ushort adcDataLength;
        public byte blackPatternFirst;
        public byte blackPatternPeriod;
        public byte pga;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct scanResults
    {
        public uint headerVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string scanName;
        public DateTimeStruct dateTime;
        public ScanDataHeadBody scanDataHeadBody;
        public SlewScanConfig config;
        public double[] wavelength;
        public int[] intensity;
        public int length;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct scanResults2
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 864)]
        public double[] wavelength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 864)]
        public int[] intensity;
        public int length;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct uScanConfig
    {
        [FieldOffset(0)]
        public ScanConfig ScanCfg;
        [FieldOffset(0)]
        public SlewScanConfig SlewScanCfg;
    }
}
