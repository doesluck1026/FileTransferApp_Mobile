using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

class Main
{
    #region Parameters

    private readonly byte StartByte = (byte)'J';
    private readonly int BufferSize = 64 * 1024-1;
    private readonly int Port = 32001;

    #endregion

    #region Public Variables

    public string ServerIP;
    public string ClientIP;
    public FileStruct CurrentFile;

    private bool IsTransferEnabled
    {
        get
        {
            lock (Lck_IsTransferEnabled)
                return _isTransferEnabled;
        }
        set
        {
            lock (Lck_IsTransferEnabled)
                _isTransferEnabled = value;
        }
    }
    #endregion

    #region Private Variables

    private Client client;
    private Server server;
    private FileOperations File;
    private string[] FilePaths;
    private string[] FileNames;                 /// Name of Files
    private long[] FileSizeAsBytes;             /// Size of files as bytes
    private double[] FileSizes;                 /// File Sizes as a double 
    private FileOperations.SizeUnit[] SizeUnits;  /// Unit of filesizes
    private Thread Thread_Transfer;
    private bool _isTransferEnabled = false;
    #endregion

    #region Lock Objects
    private object Lck_IsTransferEnabled = new object();
    #endregion

    #region Enums and structures definitions
    public enum Functions
    {
        QueryTransfer,              /// ||Number of files || Size of All Data (8 bytes) || length of folder name (zero : if no folder given) || name of folder ==> true or false as response
        StartofFileTransfer,        /// || length of file name || name || size of file (8 bytes) ==> true when receiver is ready as response
        EndofFileTransfer,          /// || 4 bytes (spare) ==> no response expected   ( receiver should save the file)
        TransferMode,               /// || fileBytes
        AllisSent,                  /// || no bytes needed ==> no response expected
        AcceptFiles,                /// Response to QueryTransfer function
        RejectFiles,                /// Response to QueryTransfer function
        Ready                       /// Ready to receive file (response from receiver to StartofFileTransfer)
    }
    public struct FileStruct
    {
        public string FilePath;                     /// Path of File
        public string FileName;                     /// Name of File 
        public double FileSize;                     /// Size of file
        public FileOperations.SizeUnit SizeUnit;    /// unit of size
        public long FileSizeAsBytes;                /// Size of file as bytes
    }
    #endregion

    #region Public Functions

    public Main()
    {

    }

    public void StartServer()
    {
        server = new Server(port: Port, bufferSize: BufferSize, StartByte: StartByte);
        ServerIP = server.SetupServer();
        Task.Run(()=>
        {
            ClientIP= server.StartListener();
        });
    }
    public void SetFilePaths(string[] paths)
    {
        FilePaths = new string[paths.Length];
        paths.CopyTo(FilePaths, 0);
    }
    public bool ConnectToTargetDevice(string IP)
    {
        client=new Client(port: Port,ip: IP, bufferSize: BufferSize, StartByte: StartByte);
        client.ConnectToServer();
        SendFirstFrame();
        byte[] clientResponse = client.GetData();
        if (clientResponse == null)
            return false;
        if ((Functions)clientResponse[0] == Functions.AcceptFiles)
            return true;
        else
            return false;
    }
    public void BeginSendingFiles()
    {
        IsTransferEnabled = true;
        Thread_Transfer = new Thread(SendingCoreFcn);
        Thread_Transfer.Start();
    }
    public void AbortTransfer()
    {
        IsTransferEnabled = false;
    }
    #endregion
    private void SendingCoreFcn()
    {
        for(int i=0;i<FilePaths.Length;i++)
        {
            SendFileInformation(i);
            if (!WaitforReceiverToBeReady())
                continue;
            File = new FileOperations();
            File.Init(FilePaths[i], FileOperations.TransferMode.Send);
            CurrentFile.FilePath = File.FilePath;
            CurrentFile.FileName = File.FileName;
            CurrentFile.FileSize = File.FileSize;
            CurrentFile.FileSizeAsBytes = File.FileSizeAsBytes;
            CurrentFile.SizeUnit = File.FileSizeUnit;
            long totalBytesRead=0;
            int numberOfBytesRead=0;
            byte[] buffer;
            while (IsTransferEnabled)
            {
                File.FileReadAtByteIndex(totalBytesRead, out numberOfBytesRead, out buffer, chunkSize: BufferSize,functionByte: (byte)Functions.TransferMode);
                client.SendDataServer(buffer);
                totalBytesRead += numberOfBytesRead;
                if (totalBytesRead == File.FileSizeAsBytes)
                    break;
            }
            File.CloseFile();
            byte[] endBytes = new byte[5];
            endBytes[0] = (byte)Functions.EndofFileTransfer;
            client.SendDataServer(endBytes);
            if (!IsTransferEnabled)
                break;
        }
        SendLastFrame();
        client.DisconnectFromServer();
        client = null;
    }
    private void SendFirstFrame()
    {
        long totalTransferSize = GetTransferSize();
        byte[] data = new byte[10];
        data[0] =(byte)Functions.QueryTransfer;
        byte[] sizeBytes = BitConverter.GetBytes(totalTransferSize);
        sizeBytes.CopyTo(data, 1);
        client.SendDataServer(data);
    }
    private void SendLastFrame()
    {
        byte[] data = new byte[2];
        data[0] = (byte)Functions.AllisSent;
        client.SendDataServer(data);
    }
    private void SendFileInformation(int indexOfFile)
    {
        byte[] nameBytes = Encoding.ASCII.GetBytes(FileNames[indexOfFile]);
        int nameLen = nameBytes.Length;
        byte[] data = new byte[nameLen + 10];
        data[0] = (byte)Functions.StartofFileTransfer;
        data[1] = (byte)nameLen;
        nameBytes.CopyTo(data, 2);
        byte[] lengthBytes=BitConverter.GetBytes(FileSizeAsBytes[indexOfFile]);
        lengthBytes.CopyTo(data,nameLen + 2);
        client.SendDataServer(data);
    }
    private bool WaitforReceiverToBeReady()
    {
        byte[] receivedData = client.GetData();
        if (receivedData == null)
            return false;
        if ((Functions)receivedData[0] == Functions.Ready)
            return true;
        else
            return false;
    }
    private long GetTransferSize()
    {
        long totalTransferSize = 0;     // bytes
        File = new FileOperations();
        for (int i = 0; i < FilePaths.Length; i++)
        {
            File.Init(FilePaths[i], FileOperations.TransferMode.Send);
            totalTransferSize += File.FileSizeAsBytes;
            FileNames[i] = File.FileName;
            FilePaths[i] = File.FilePath;
            FileSizeAsBytes[i] = File.FileSizeAsBytes;
            FileSizes[i] = File.FileSize;
            SizeUnits[i] = File.FileSizeUnit;
            File.CloseFile();
        }
        return totalTransferSize;
    }
}
