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
        private readonly uScanConfig scanConfig=new uScanConfig();
        
        public static void WriteScanCFG()
        {
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
        public static void SetNumOfRepeats()
        {
            // Attempt to set the active scan index
            int result = APILib.NNO_SetScanNumRepeats(1);
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
    
        public static void InterpretResult()
        {

        }
    }
}
