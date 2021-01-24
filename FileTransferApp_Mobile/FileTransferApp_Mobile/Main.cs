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
    private readonly int BufferSize = 64 * 1024;
    private readonly int Port = 32001;

    #endregion

    #region Public Variables
    public string ServerIP;
    public string ClientIP;
    #endregion

    #region Private Variables
    private Client client;
    private Server server;
    private FileOperations File;
    #endregion

    #region Lock Objects

    #endregion

    #region Public Functions

    public void StartServer()
    {
        server = new Server(port: Port, bufferSize: BufferSize, StartByte: StartByte);
        ServerIP = server.SetupServer();
        Task.Run(()=>
        {
            ClientIP= server.StartListener();
        });
    }
    
    #endregion
}
