using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp11
{
    
    public class API
    {
        private  static uScanConfig scanConfig = new uScanConfig() {
            ScanCfg = new ScanConfig()
            {
                ConfigName="TestCfg",
                WavelengthStartNm=900,
                WavelengthEndNm=1700,
                NumPatterns=1,
                NumRepeats=1,
                ScanConfigIndex=0,
                ScanType=0,
                ScanConfigSerialNumber="UB128039",
                WidthPx=2
            }
        };

        public static byte[] WriteScanCFG()
        {
            // Get the buffer size
            UIntPtr bufSize;
            DLPSPEC_ERR_CODE result = APILib.dlpspec_get_scan_config_dump_size(ref scanConfig, out bufSize);

            if (result != DLPSPEC_ERR_CODE.DLPSPEC_PASS)
            {
                throw new Exception($"Failed to get buffer size with error code: {result}");
            }

            // Allocate unmanaged memory for the buffer
            IntPtr pBuf = Marshal.AllocHGlobal((int)bufSize);

            try
            {
                // Write configuration
                result = APILib.dlpspec_scan_write_configuration(ref scanConfig, pBuf, bufSize);

                if (result != DLPSPEC_ERR_CODE.DLPSPEC_PASS)
                {
                    throw new Exception($"Failed to write configuration with error code: {result}");
                }
                Console.WriteLine(  "Configuration written");
                // Create a managed byte array to hold the data
                byte[] buffer = new byte[(int)bufSize];
                Marshal.Copy(pBuf, buffer, 0, (int)bufSize);

                return buffer;
            }
            finally
            {
                // Free the unmanaged memory
                Marshal.FreeHGlobal(pBuf);
            }
        }
        public static void ApplyScanConfig(byte[] config)
        {
            // Allocate unmanaged memory for the config data
            IntPtr pConfig = Marshal.AllocHGlobal(config.Length);

            try
            {
                // Copy the config data to the unmanaged memory
                Marshal.Copy(config, 0, pConfig, config.Length);

                // Attempt to apply the config
                int result = APILib.NNO_ApplyScanConfig(pConfig, config.Length);
                if (result < 0)
                {
                    throw new Exception("Failed to apply scan config.");
                }
            }
            finally
            {
                // Always free the unmanaged memory
                Marshal.FreeHGlobal(pConfig);
            }
        }
        public static void SetActiveScanIndex(byte index)
        {
            // Attempt to set the active scan index
            int result = APILib.NNO_SetActiveScanIndex(index);
            if (result < 0)
            {
                throw new Exception("Failed to set active scan index.");
            }
        }
        public static void SetNumOfRepeats(ushort repeats)
        {
            // Attempt to set the active scan index
            int result = APILib.NNO_SetScanNumRepeats(repeats);
            if (result < 0)
            {
                throw new Exception("Failed to set num of repeats.");
            }
        }
        public static int GetScanTime()
        {
            return APILib.NNO_GetEstimatedScanTime();
        }
        public static void PerformScan(bool storeInSDCard)
        {
            int result = APILib.NNO_PerformScan(storeInSDCard);
            if (result < 0)
            {
                throw new Exception("Failed to perform scan.");
            }
        }
        public static bool ScanCompleted()
        {
            int result=APILib.NNO_GetScanComplete();
            if (result < 0)
            {
                throw new Exception("Failed to check scan completion.");
            }
            return result == 1;

        }

        public static byte[] GetFileData()
        {
            int sizeInBytes = 0;
            // Assume we're trying to get scan data
            int result = APILib.NNO_GetFileSizeToRead(NNO_FILE_TYPE.NNO_FILE_SCAN_DATA);
            if (result < 0)
            {
                throw new Exception("Failed to get file size.");
            }

            sizeInBytes = result;
            // Allocate unmanaged memory for the data
            IntPtr pData = Marshal.AllocHGlobal(sizeInBytes);

            try
            {
                // Attempt to get the data
                result = APILib.NNO_GetFileData(pData, ref sizeInBytes);
                if (result < 0)
                {
                    throw new Exception("Failed to get file data.");
                }

                // If successful, copy the data to a managed array
                byte[] data = new byte[sizeInBytes];
                Marshal.Copy(pData, data, 0, sizeInBytes);
                return data;
            }
            finally
            {
                // Always free the unmanaged memory
                Marshal.FreeHGlobal(pData);
            }
        }

        public static scanResults InterpretScanData(byte[] byteArray)
        {
            int size = byteArray.Length;
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(byteArray, 0, ptr, size);

            scanResults results = new scanResults();
            DLPSPEC_ERR_CODE errCode = APILib.dlpspec_scan_interpret(ptr, (UIntPtr)size, ref results);

            if (errCode != DLPSPEC_ERR_CODE.DLPSPEC_PASS)
            {
                // handle error
                Console.WriteLine("Error interpreting scan data. Error Code: {0}", errCode);
            }

            Marshal.FreeHGlobal(ptr);

            return results;
        }
        //additional
        private static byte[] StructToBytes(uScanConfig config)
        {
            int size = Marshal.SizeOf(config);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(config, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }
}
