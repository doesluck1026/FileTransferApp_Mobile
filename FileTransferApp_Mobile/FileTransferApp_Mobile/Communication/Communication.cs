using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Communication pack structure should be like below:
///     [Header Pack= 7 bytes long] + [Data Bytes = (2^40)-1 bytes long]
/// Header Pack Data Structure:
///     [Start Byte] + [Function Byte] + [Spare Byte] + [Length Bytes=4 bytes Length]
///         -> Start byte is ascii 74 character and same in every packs
///         -> Function Byte specifies what the whole pack is about and can be one of the Functions enum which defined below.
///         -> Length bytes specifies the length of the bytes that come after header bytes. these bytes are encoded as little endien.
/// Data Bytes Data Structure:
///     [region1 length= 2 bytes] + [region1 bytes] + [region2 length= 2 bytes] + [region2 bytes]....
/// </summary>


class Communication
{
    private static Server server;
    private static Client client;
    private static readonly byte StartByte = (byte)'J';
    private static int Port = 41001;
    private static int BufferSize = 1024 * 64;

    public static bool isClientConnected = false;
    public static bool isConnectedToServer = false;
    public static long LastPackNumberReceived { get; private set; }
    public static long LastPackNumberSent { get; private set; }
    public static uint NumberOfPacks { get; private set; }
    public static bool isFileReceived { get; private set; }
    public static string TransferCode { get; private set; }

    private enum Functions
    {
        QueryTransfer = 2,
        SendingFile = 3,
        FileSent = 4,
        TransferStatus = 5,
    }
    public static void Init()
    {
        LastPackNumberSent = 0;
        LastPackNumberReceived = -1;
        client = new Client();
    }

    #region Common Functions
    private static byte[] PrepareDataHeader(Functions func, uint len)
    {
        byte[] HeaderBytes = new byte[5];
        HeaderBytes[0] = StartByte;
        HeaderBytes[1] = Convert.ToByte(func);
        byte[] lenBytes = BitConverter.GetBytes(len);
        Array.Copy(lenBytes, 0, HeaderBytes, 3, lenBytes.Length);
        return HeaderBytes;
    }
    private static uint CalculatePackageCount(double fileSize, FileOperations.SizeUnit sizeType)
    {
        long PackageSize = (uint)(fileSize * Math.Pow(1024, (double)sizeType));
        uint packageCount = (uint)Math.Ceiling(PackageSize / 5000.0);
        return packageCount;
    }
    #endregion
}
