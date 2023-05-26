// See https://aka.ms/new-console-template for more information
using ConsoleApp11;
using System;
/*
typedef enum
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
NNO_FILE_TYPE;
*/
//1. write_scan_configuration()
//2. NNO_CMD_SCAN_CFG_APPLY | int NNO_ApplyScanConfig	(	void * 	pBuffer,int bufSize )		
//3. NNO_CMD_SCAN_SET_ACT_CFG  | int NNO_SetActiveScanIndex(uint8 index)
// возможно нужно один из двух (или 2 или 3)

//4. (optional) NNO_CMD_SCAN_NUM_REPEATS | int NNO_SetScanNumRepeats(uint16 num)
//5. NNO_CMD_READ_SCAN_TIME | int NNO_GetEstimatedScanTime	(void)	
//6. Imporant:   NNO_CMD_PERFORM_SCAN | int NNO_PerformScan(bool StoreInSDCard)
//7. NNO_CMD_SCAN_GET_STATUS (в user guide написано NNO_CMD_READ_DEVICE_STATUS хотя такой команды нет) |
// int NNO_GetScanComplete(void)

//8. NNO_CMD_FILE_GET_READSIZE | int NNO_GetFileSizeToRead(NNO_FILE_TYPE fileType) filetype=NNO_FILE_SCAN_DATA|0

//9. NNO_CMD_FILE_GET_DATA | int NNO_GetFileData(void* pData,int* pSizeInBytes)
//10. dlpspec_scan_interpret() 


Console.WriteLine(APILib.USB_Open());
Console.WriteLine(APILib.USB_IsConnected());
Console.WriteLine(API.SetHibernate(false)); 

var conf=API.WriteScanCFG();
API.ApplyScanConfig(conf);
API.SetActiveScanIndex(0);
API.SetNumOfRepeats(1);
int time = API.GetScanTime();
Console.WriteLine(  time);

API.PerformScan(false);
while (!API.ScanCompleted())
{
    Thread.Sleep(100);
}
byte[]data=API.GetFileData();
var scanres=API.InterpretScanData(data);
Console.WriteLine( $"Length: {scanres.length}" );
for (int i = 0; i < scanres.wavelength.Length; i++)
{
    Console.WriteLine($"Wavelength: {scanres.wavelength[i]}    Intensity:{scanres.intensity[i]}" );
}
Console.WriteLine(APILib.USB_Close());