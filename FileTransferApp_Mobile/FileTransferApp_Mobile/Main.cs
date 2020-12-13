﻿using System;
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
    public enum ProccessType
    {
        SendFile,
        ReceiveFile
    }

    #region Parameters
    public static int PackSize = 1024 * 1024 * 3;            /// this represents the maximum length of bytes to be transfered to client in one package. default is 3 MB and should be smaller than 64 kB

    #endregion

    #region Variables

    private static string _URL;                          /// File Path
    private static Thread sendingThread;
    private static FileOperations FileOps;
    private static Thread receivingThread;
    private static object speedLock = new object();
    private static object completedLock = new object();
    private static object transferApprovedLock = new object();
    private static object transferabortedLock = new object();
    private static object HostName_Lock = new object();
    private static object IpCode_Lock = new object();
    private static object URL_Lock = new object();
    private static object InfoMsg_Lock = new object();
    private static object EstimatedMin_Lock = new object();
    private static object EstimatedSec_Lock = new object();
    private static object PassedMin_Lock = new object();
    private static object PassedSec_Lock = new object();
    private static object FileName_Lock = new object();
    private static object FileSize_Lock = new object();
    private static object FileSizeType_Lock = new object();
    private static object FirstStep_Lock = new object();
    private static object SecondStep_Lock = new object();
    private static object ThirdStep_Lock = new object();
    private static object ExportingVerification_Lock = new object();

    private static string _IpCode = "";
    private static string _HostName = "";
    private static bool _TransferVerified = false;
    private static double _transferSpeed = 0;
    private static double _completedPercentage = 0;
    private static bool _transferApproved = false;
    private static bool _transferAborted = false;
    private static string _InfoMsg = "";
    private static uint prev_timePassed = 0;
    private static int _estimatedMin = 0;
    private static int _estimatedSec = 0;
    private static int _passedMin = 0;
    private static int _passedSec = 0;
    private static string _FileName = "";
    private static double _FileSize = 0;
    private static Communication.SizeTypes _FileSizeType;
    private static bool _FirstStep_Action = false;
    private static bool _SecondStep_Action = false;
    private static bool _ThirdStep_Action = false;
    private static bool _ExportingVerification = false;

    public static double TransferSpeed
    {

        get
        {
            lock (speedLock)
            {
                return _transferSpeed;
            }
        }
        set
        {
            lock (speedLock)
            {
                _transferSpeed = value;
            }
        }
    }
    public static double CompletedPercentage
    {

        get
        {
            lock (completedLock)
            {
                return _completedPercentage;
            }
        }
        set
        {
            lock (completedLock)
            {
                _completedPercentage = value;
            }
        }
    }
    public static bool TransferApproved
    {

        get
        {
            lock (transferApprovedLock)
            {
                return _transferApproved;
            }
        }
        set
        {
            lock (transferApprovedLock)
            {
                _transferApproved = value;
            }
        }
    }
    public static bool TransferAborted
    {

        get
        {
            lock (transferabortedLock)
            {
                return _transferAborted;
            }
        }
        set
        {
            lock (transferabortedLock)
            {
                _transferAborted = value;
            }
        }
    }
    public static string HostName
    {

        get
        {
            lock (HostName_Lock)
            {
                return _HostName;
            }
        }
        set
        {
            lock (HostName_Lock)
            {
                _HostName = value;
            }
        }
    }
    public static string IpCode
    {

        get
        {
            lock (IpCode_Lock)
            {
                return _IpCode;
            }
        }
        set
        {
            lock (IpCode_Lock)
            {
                _IpCode = value;
            }
        }
    }
    public static string URL
    {

        get
        {
            lock (URL_Lock)
            {
                return _URL;
            }
        }
        set
        {
            lock (URL_Lock)
            {
                _URL = value;
            }
        }
    }
    public static string FileName
    {

        get
        {
            lock (FileName_Lock)
            {
                return _FileName;
            }
        }
        set
        {
            lock (FileName_Lock)
            {
                _FileName = value;
            }
        }
    }
    public static double FileSize
    {

        get
        {
            lock (FileSize_Lock)
            {
                return _FileSize;
            }
        }
        set
        {
            lock (FileSize_Lock)
            {
                _FileSize = value;
            }
        }
    }
    public static Communication.SizeTypes FileSizeType
    {

        get
        {
            lock (FileSizeType_Lock)
            {
                return _FileSizeType;
            }
        }
        set
        {
            lock (FileSizeType_Lock)
            {
                _FileSizeType = value;
            }
        }
    }
    public static string InfoMsg
    {

        get
        {
            lock (InfoMsg_Lock)
            {
                return _InfoMsg;
            }
        }
        set
        {
            lock (InfoMsg_Lock)
            {
                _InfoMsg = value;
            }
        }
    }
    public static int EstimatedMin
    {

        get
        {
            lock (EstimatedMin_Lock)
            {
                return _estimatedMin;
            }
        }
        set
        {
            lock (EstimatedMin_Lock)
            {
                _estimatedMin = value;
            }
        }
    }
    public static int EstimatedSec
    {

        get
        {
            lock (EstimatedSec_Lock)
            {
                return _estimatedSec;
            }
        }
        set
        {
            lock (EstimatedSec_Lock)
            {
                _estimatedSec = value;
            }
        }
    }
    public static int PassedMin
    {

        get
        {
            lock (PassedMin_Lock)
            {
                return _passedMin;
            }
        }
        set
        {
            lock (PassedMin_Lock)
            {
                _passedMin = value;
            }
        }
    }
    public static int PassedSec
    {

        get
        {
            lock (PassedSec_Lock)
            {
                return _passedSec;
            }
        }
        set
        {
            lock (PassedSec_Lock)
            {
                _passedSec = value;
            }
        }
    }

    public static bool ThirdStep
    {

        get
        {
            lock (ThirdStep_Lock)
            {
                return _ThirdStep_Action;
            }
        }
        set
        {
            lock (ThirdStep_Lock)
            {
                _ThirdStep_Action = value;
            }
        }
    }

    public static bool SecondStep
    {

        get
        {
            lock (SecondStep_Lock)
            {
                return _SecondStep_Action;
            }
        }
        set
        {
            lock (SecondStep_Lock)
            {
                _SecondStep_Action = value;
            }
        }
    }

    public static bool FirstStep
    {

        get
        {
            lock (FirstStep_Lock)
            {
                return _FirstStep_Action;
            }
        }
        set
        {
            lock (FirstStep_Lock)
            {
                _FirstStep_Action = value;
            }
        }
    }

    public static bool ExportingVerification
    {

        get
        {
            lock (ExportingVerification_Lock)
            {
                return _ExportingVerification;
            }
        }
        set
        {
            lock (ExportingVerification_Lock)
            {
                _ExportingVerification = value;
            }
        }
    }

    #endregion


    public static void Init(bool isFirstTime)
    {
        Communication.Init();
        if (isFirstTime)
            FileOps = new FileOperations();

        InfoMsg = "sSelectAction";

    }
    public static void CloseServer()
    {
        Communication.CloseServer();
    }
    public static void CloseClient()
    {
        Communication.CloseClient();
    }
    private static void CalculateCompletedPercentage(uint numPack = 0)
    {
        uint NumberOfPacks = Communication.NumberOfPacks;
        if (NumberOfPacks != 0)
        {
            double _completedPercentage = (((double)numPack / NumberOfPacks) * 100);
            CompletedPercentage = _completedPercentage;
        }
        return;
    }

    public static void Reset()
    {

        CloseServer();
        CloseClient();
        if (sendingThread != null)
        {
            if (sendingThread.IsAlive)
            {
                TransferAborted = true;
                Thread.Sleep(5);
                sendingThread.Abort();
            }
        }
        ExportingVerification = false;
        TransferAborted = false;
        FileOps.CloseFile();
        FileName = "";
        URL = "";
        HostName = "";
        FileSize = 0;
        FileSizeType = Communication.SizeTypes.none;
    }
    private static double TotalMegaBytesSent = 0;
    private static void CalculateEstimatedTime(long numBytes, uint numPack, uint TimePassed)
    {
        uint MB = 1024 * 1024;
        uint ETA;
        uint NumberOfPacks = Communication.NumberOfPacks;
        double numMegaBytes = (double)numBytes / MB;
        TotalMegaBytesSent += numMegaBytes;

        double deltaTime = (TimePassed - prev_timePassed) / 1000.0;
        prev_timePassed = TimePassed;
        TimePassed /= 1000;
        double averageSpeed = TotalMegaBytesSent / TimePassed;
        TransferSpeed = TransferSpeed * 0.8 + 0.2 * (numMegaBytes / deltaTime);
        ETA = (uint)((((NumberOfPacks - numPack) * Main.PackSize / MB) / averageSpeed));
        if (TransferSpeed > 500 || TransferSpeed < 0)
            TransferSpeed = 0;
        EstimatedMin = (int)(ETA / 60);
        EstimatedSec = (int)(ETA % 60);
        PassedMin = (int)(TimePassed / 60.0);
        PassedSec = (int)TimePassed % 60;
    }

    #region Server Functions

    /// <summary>
    /// Gets the Selected URL and setups server
    /// </summary>
    /// <param name="url">Path of the file to be transfered</param>
    /// <returns></returns>
    public static void SetFileURL(string filePath)
    {
        TotalMegaBytesSent = 0;
        URL = filePath;                                                  /// assign URL
        FileOps.Init(filePath, FileOperations.TransferMode.Send);
        FileName = FileOps.FileName;
        FileSize = FileOps.FileSize;
        FileSizeType = FileOps.FilesizeType;
        FirstStep = true;
        WaitForConnection();                /// Setup the server and accept connection

    }
    /// <summary>
    /// This function is used to send information to client about the file to be transfered and queries if client still wants to receive the file
    /// Call this Function after User confirms to send file.
    /// </summary>
    /// <returns> returns acception status according to answer of the client (true or false)</returns>
    public static bool QueryForTransfer()
    {
        bool isAccepted = Communication.QueryForTransfer(FileOps.FileName, FileOps.FileSize, FileOps.FilesizeType);
        return false;                                                       /// return false
    }
    /// <summary>
    /// Starts sending slected file to client in another thread.
    /// </summary>
    /// <returns>returns true if transfer is started</returns>
    private static bool StartFileTransfer()
    {
        try
        {
            sendingThread = new Thread(SendingCoreFcn);                             /// Start Sending File
            sendingThread.Start();
            string Msg = "sWaitClient"; //resMng.GetString("sWaitClient", culInfo);  // "Wait for Client.";
            InfoMsg = Msg;
            Debug.WriteLine("Wait for Client.");
            return true;
        }
        catch (Exception e)
        {
            string Msg = "sFailedClient"; // "Failed to start sending thread!";
            InfoMsg = Msg;
            Debug.WriteLine("Failed to start sending thread! \n " + e.ToString());
            return false;
        }
    }
    /// <summary>
    /// Sets up the server and starts listening to clients
    /// </summary>
    /// <returns>returns true if a client successfully connected</returns>
    private static void WaitForConnection()
    {
        string IpCode_ = Communication.CreateServer();                     /// setup the server and start listening to port
        IpCode = IpCode_;
        //event_UpdateUI(_IpCode, _HostName, _TransferVerified);      /// display event
        bool isTransferStarted = StartFileTransfer();                     /// Start File Transfer
    }
    /// <summary>
    /// This function is used in a thread to send all file bytes to client.
    /// </summary>
    private static void SendingCoreFcn()
    {
        if (FileOps != null)
        {
            string clientHostname = Communication.startServer();            /// Wait for Client to connect and return the hostname of connected client.
            HostName = clientHostname;

            ExportingVerification = true;


            while (!TransferApproved && !TransferAborted) ;
            if (TransferAborted)
            {
                string Msg = "sTransferAborted"; // "Transfer is aborted by user!";
                InfoMsg = Msg;
                Debug.WriteLine("Transfer is aborted by user!");
                sendingThread.Abort();
                sendingThread = null;
                return;
            }

            if (clientHostname != null && clientHostname != "")             /// if connection succeed
            {
                bool isVerified = Communication.VerifyCode();
                Debug.WriteLine("isVerified: " + isVerified);
                if (isVerified)
                {
                    QueryForTransfer();
                    _TransferVerified = isVerified;
                    HostName = clientHostname;
                    string Msg = "sVerifiedSuccess"; //"isVerified: " + isVerified;
                    InfoMsg = Msg;
                }
                else
                {
                    string Msg = "sVerifiedFailed"; // "isVerified: " + isVerified + " aborting!";
                    InfoMsg = Msg;
                    Debug.WriteLine("isVerified: " + isVerified + " Aborting!");
                    Communication.RejectClient();
                    Communication.CloseServer();
                    return;
                }
            }
            else
            {
                string Msg = "sClientHostNameNull"; // "Client host name was null. Aborting!";
                InfoMsg = Msg;
                Communication.CloseServer();
                Debug.WriteLine("clientHostname was null. Aborting!");
                return;
            }
            /// define variables
            long bytesSent = 0;
            int BytesRead = 0;
            long numPack = 0;
            bool isSent = true;
            byte[] BytesToSend;                                                                     /// Define byte array to carry file bytes
            uint numberOfPacks = Communication.NumberOfPacks;
            long checkPoint = 0;
            uint TimePassed = 0;
            uint elapsedTime = 0;
            long numBytesSent = 0;
            SecondStep = true;
            InfoMsg = "sFileBeingSent";
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (bytesSent < FileOps.FileSizeAsBytes)                                               /// while the number of bytes sent to client is smaller than the total file length
            {
                FileOps.FileReadAtByteIndex(bytesSent, out BytesRead, out BytesToSend, PackSize);     /// read file and copy to carrier array.
                isSent = Communication.SendFilePacks(BytesToSend, numPack);                         /// send the bytes
                if (isSent)
                {
                    numPack++;                                                                      /// increase the number of package sent variable
                    bytesSent += BytesRead;                                                         /// update the number of bytes sent to client.
                }
                else
                {
                    numPack = Communication.LastPackNumberReceived;
                    bytesSent = numPack * PackSize;
                    Debug.WriteLine("Last Package Sent: " + Communication.LastPackNumberSent + "  Last pack received: " + Communication.LastPackNumberReceived);
                    Debug.WriteLine("Could not send Last Package! Retrying...");
                }
                elapsedTime = (uint)stopwatch.ElapsedMilliseconds;
                if (elapsedTime >= 50)
                {
                    TimePassed += elapsedTime;
                    numBytesSent = bytesSent - checkPoint;
                    checkPoint = bytesSent;
                    CalculateCompletedPercentage((uint)numPack);
                    CalculateEstimatedTime(numBytesSent, (uint)numPack, TimePassed);
                    stopwatch.Restart();
                }
            }
            CalculateCompletedPercentage((uint)numPack);
            CalculateEstimatedTime(numBytesSent, (uint)numPack, TimePassed);
            if (isSent)                                                                             /// if all file is sent
            {
                string Msg = "sSendingSuccess"; //"File is Succesfully Sent";
                InfoMsg = Msg;
                Communication.CompleteTransfer();                                                   /// stop data transfer and let client know that the transfer is successfully done.
                ThirdStep = true;
                Debug.WriteLine("File is Succesfully Sent");
            }
            else
            {
                string Msg = "sSendingFailed"; // "File Transfer Failed!";
                InfoMsg = Msg;
                Debug.WriteLine("File Transfer Failed!");
            }
            FileOps.CloseFile();
            Communication.CloseServer();
        }
    }

    #endregion

    #region Client Functions
    public static void SetFilePathToSave(string path)
    {
        URL = path;
        FileOps.Init(path, FileOperations.TransferMode.Receive);
        TotalMegaBytesSent = 0;
        //RespondToTransferRequest(true);
    }
    public static bool EnterTheCode(string code)
    {
        bool success = false;
        string serverIP = Communication.DecodeTransferCode(code);
        if (serverIP != null && serverIP != "")
        {
            bool isConnected = Communication.ConnectToServer(serverIP);
            if (isConnected)
            {
                Communication.SendVerification(code);
                string fileName;
                double fileSize;
                bool isCodeIncorrect;
                Communication.SizeTypes sizeType;
                Communication.GetFileSpecs(out fileName, out fileSize, out sizeType, out isCodeIncorrect);
                if (isCodeIncorrect)
                    return false;
                FileOps.FileName = fileName;
                FileName = fileName;
                FileSize = fileSize;
                FileSizeType = sizeType;
                Debug.WriteLine("fileName: " + fileName + "   FileOps.FileName: " + FileOps.FileName);
                // Show Specs to user and ask for permission
                success = true;
            }
        }
        return success;
    }
    public static bool RespondToTransferRequest(bool isAccepted)
    {
        Communication.RespondToTransferRequest(isAccepted);
        if (isAccepted)
            return StartReceiving();
        else
            return false;
    }
    private static bool StartReceiving()
    {
        SecondStep = true;
        try
        {
            receivingThread = new Thread(ReceivingCoreFcn);
            receivingThread.Start();
            Debug.WriteLine("File Transfer is Started");
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine("Failed to Start sending thread! \n " + e.ToString());
            return false;
        }
    }
    private static void ReceivingCoreFcn()
    {
        if (FileOps != null)
        {
            /// define variables
            long bytesWritten = 0;
            long numPack = 0;
            byte[] BytesToWrite;                                                                     /// Define byte array to carry file bytes
            uint numberOfPacks = Communication.NumberOfPacks;
            long checkPoint = 0;
            uint TimePassed = 0;
            uint elapsedTime = 0;
            long numBytesSent = 0;
            Main.InfoMsg = "sFileBeingReceived";
            Stopwatch stopwatch = Stopwatch.StartNew();
            Debug.WriteLine(" Communication.NumberOfPacks: " + numberOfPacks);
            while (numPack < numberOfPacks)                                           /// while the number of bytes sent to client is smaller than the total file length
            {
                BytesToWrite = Communication.ReceiveFilePacks();
                if (BytesToWrite == null)
                {
                    Debug.WriteLine("BytesToWrite is null");
                    continue;
                }
                FileOps.FileWriteAtByteIndex(bytesWritten, BytesToWrite);                                /// read file and copy to carrier array.
                numPack++;                                                                              /// increase the number of package sent variable
                bytesWritten += BytesToWrite.Length;                                                    /// update the number of bytes sent to client.
                elapsedTime = (uint)stopwatch.ElapsedMilliseconds;
                if (elapsedTime >= 50)
                {
                    TimePassed += elapsedTime;
                    numBytesSent = bytesWritten - checkPoint;
                    checkPoint = bytesWritten;
                    CalculateCompletedPercentage((uint)numPack);
                    CalculateEstimatedTime(numBytesSent, (uint)numPack, TimePassed);
                    stopwatch.Restart();
                }
            }
            CalculateCompletedPercentage((uint)numPack);
            CalculateEstimatedTime(numBytesSent, (uint)numPack, TimePassed);
            if (numPack == numberOfPacks)
            {
                string Msg = "sFileReceivingSuccess"; // "File is succesfully received!";
                InfoMsg = Msg;
                Debug.WriteLine("File is Succesfully Received");
                ThirdStep = true;

            }
            FileOps.CloseFile();
            Communication.CloseClient();
        }
    }

    #endregion
}
