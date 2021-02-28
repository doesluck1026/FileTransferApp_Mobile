using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class NetworkScanner
{
    public delegate void ScanCompleteDelegate();
    public static event ScanCompleteDelegate OnScanCompleted;
    public static List<string> DeviceNames = new List<string>();
    public static List<string> DeviceIPs = new List<string>();
    private static int ConnectionTimeout;
    public static bool IsScanning
    {
        get
        {
            lock (Lck_IsScanning)
                return _isScanning;
        }
        private set
        {
            lock (Lck_IsScanning)
                _isScanning = value;
        }
    }
    public static int ScanPercentage
    {
        get
        {
            lock (Lck_ScanPercentage)
                return _scanPercentage;
        }
        private set
        {
            lock (Lck_ScanPercentage)
                _scanPercentage = value;
        }
    }

    private static int[] scanProgressArr;
    private static string DeviceIP;
    private static readonly int PublishPort = 42019;
    private static Server publisherServer;

    private static string IPHeader;
    public static bool IsDevicePublished = false;

    private static bool _isScanning = false;
    private static int _scanPercentage = 0;

    private static object Lck_IsScanning = new object();
    private static object Lck_ScanPercentage = new object();

    private static int ScanCounter = 0;
    public static void ScanAvailableDevices(int timeout=200)
    {
        ConnectionTimeout = timeout;
        ScanPercentage = 0;
        string deviceIP, deviceHostname;
        GetDeviceAddress(out deviceIP, out deviceHostname);
        DeviceIP = deviceIP;
        DeviceNames.Clear();
        DeviceIPs.Clear();
        char[] splitter = new char[] { '.' };
        var ipStack = deviceIP.Split(splitter);
        IPHeader = "";
        for (int i = 0; i < 3; i++)
        {
            IPHeader += ipStack[i] + ".";
        }
        
        IsScanning = true;
        Task.Run(() =>
        {
            int numTasks = 10;
            int stackSize = 256 / numTasks;
            scanProgressArr = new int[numTasks];
            for (int i = 0; i < numTasks; i++)
            {
                ParallelScan(stackSize * i + 1, stackSize * (i + 1) + 1, i);
               // Debug.WriteLine("i: "+ i+"  stx:"+ (stackSize * i + 1)+" endx: "+(stackSize * (i + 1) + 1));
            }
            Task.Run(() =>
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (true)
                {
                    int percentage = 0;
                    for (int i = 0; i < numTasks; i++)
                    {
                        percentage += scanProgressArr[i];
                    }
                    percentage /= numTasks;
                    //Debug.WriteLine("percentage: " + percentage);
                    ScanPercentage = percentage;
                    if (percentage >= 99)
                        break;
                    Thread.Sleep(50);
                }
                stopwatch.Stop();
                Debug.WriteLine("scan time: " + stopwatch.Elapsed.TotalSeconds + " s");
                if (OnScanCompleted != null)
                    OnScanCompleted();
                else
                {
                    ScanCounter++;
                    if (ScanCounter < 3 && DeviceIPs.Count < 1)
                        ScanAvailableDevices();
                    else
                        ScanCounter = 3;
                }
                IsScanning = false;
            });
        });
        
    }

    private static void ParallelScan(int startx,int endx,int progressIndex)
    {
        Task.Run(() =>
        {
            Stopwatch stp = Stopwatch.StartNew();
            int progress = 0;
            for (int i = startx; i < endx; i++)
            {
                try
                {
                    //Debug.WriteLine("Pinging: " + holder.IpHeader + i.ToString());
                    string targetIP = IPHeader + i.ToString();
                    if (targetIP == DeviceIP)
                        continue;
                    GetDeviceData(targetIP);
                    progress= (int)(((i - startx) / (double)(endx - startx-1)) * 100.0);
                    scanProgressArr[progressIndex] = progress;
                   // Debug.WriteLine("index: "+progressIndex+" progress: "+ progress);
                }
                catch
                {

                }
            }
            
        });
    }
    private static void GetDeviceData(string IP)
    {
        //Stopwatch stp = Stopwatch.StartNew();
        var client = new Client(port: PublishPort, ip: IP);
        string clientIP = client.ConnectToServer(ConnectionTimeout);
        if (string.IsNullOrEmpty(clientIP))
        {
            //Debug.WriteLine("Connection Failed on: " + IP);
        }
        else
        {
            var data = client.GetData();
            if (data == null)
            {
                Debug.WriteLine("Data was null: " + IP);
                return;
            }
            string msg = Encoding.ASCII.GetString(data);
            char[] splitter = new char[] { '&' };
            string[] msgParts = msg.Split(splitter);
            string ip = msgParts[0].Substring(3);
            string deviceName = msgParts[1].Substring(11);
            DeviceNames.Add(deviceName);
            DeviceIPs.Add(ip);
            Debug.WriteLine("data: " + msg);
            client.SendDataServer(Encoding.ASCII.GetBytes("Gotcha"));
            client.DisconnectFromServer();
        }
    }
    public static void PublishDevice()
    {
        publisherServer = new Server(port: PublishPort);
        publisherServer.SetupServer();
        publisherServer.StartListener();
        publisherServer.OnClientConnected += PublisherServer_OnClientConnected;
        Debug.WriteLine("Publisher started!");
        IsDevicePublished = true;
    }

    private static void PublisherServer_OnClientConnected(string clientIP)
    {
        Debug.WriteLine("Client IP: " + clientIP);
        publisherServer.SendDataToClient(Encoding.ASCII.GetBytes("IP:" + DeviceIP + "&DeviceName:" + Parameters.DeviceName));

        publisherServer.GetData();

        publisherServer.CloseServer();
        PublishDevice();
    }

    public static void GetDeviceAddress(out string deviceIP, out string deviceHostname)
    {
        IPAddress localAddr = null;
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localAddr = ip;
            }
        }
        deviceIP = localAddr.ToString();
        deviceHostname = host.HostName;
        DeviceIP = localAddr.ToString();
    }
    public string GetHostName(string ipAddress)
    {
        try
        {
            IPHostEntry entry = Dns.GetHostEntry(ipAddress);
            if (entry != null)
            {
                return entry.HostName;
            }
        }
        catch (SocketException)
        {
            // MessageBox.Show(e.Message.ToString());
        }

        return null;
    }
    public string GetDeviceHostName()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        return host.HostName;
    }
}
class PublisherServer
{
    #region Definitions
    private int BufferSize;
    private TcpListener Listener;
    private TcpClient Client;
    private int Port;
    private string IP;
    private byte StartByte;
    public bool IsCLientConnected = false;
    public bool IsServerStarted = false;

    public delegate void ClientConnectedDelegate(string clientIP);
    public event ClientConnectedDelegate OnClientConnected;
    #endregion

    public PublisherServer(int port = 38000, string ip = "", int bufferSize = 1024 * 64, byte StartByte = (byte)'A')
    {
        this.Port = port;
        this.IP = ip;
        this.BufferSize = bufferSize;
        this.StartByte = StartByte;
    }
    public string SetupServer()
    {
        try
        {
            IPAddress localAddr = null;
            if (string.IsNullOrEmpty(IP))
            {
                var ipAddresses = GetAllInternetworkIPs();
                if (ipAddresses.Length > 0)
                    localAddr = ipAddresses[0];
                else
                {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            localAddr = ip;
                        }
                    }
                }
            }
            else
                localAddr = IPAddress.Parse(IP);
            Listener = new TcpListener(localAddr, Port);
            Listener.Start();
            Task.Run(() => StartListener());
            IsServerStarted = true;
            IP = localAddr.ToString();
            Debug.WriteLine("Server is ready:  IP: " + IP);
            return IP;
        }
        catch (Exception e)
        {
            Debug.WriteLine("Failed to Start Server!" + e.ToString());
            return null;
        }
    }
    public void StartListener()
    {
        try
        {
            if (Listener == null)
                return;
            Debug.WriteLine("Listener is Started IP:  " + IP + "  Port: " + Port);
            //Listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), Listener);
            while (true)
            {
                Debug.WriteLine("Waiting for new connection...");

                Client = Listener.AcceptTcpClient();
                IPEndPoint endPoint = (IPEndPoint)Client.Client.RemoteEndPoint;
                var ipAddress = endPoint.Address;
                Client.ReceiveBufferSize = BufferSize;
                Client.SendBufferSize = BufferSize;
                Debug.WriteLine(ipAddress + " is connected");
                OnClientConnected(ipAddress.ToString());
            }
        }
        catch
        {
        }
    }
    public void DoAcceptTcpClientCallback(IAsyncResult ar)
    {
        try
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            if (listener.Server == null)
                return;
            if (listener.Server.LocalEndPoint == null)
                return;
            Client = listener.EndAcceptTcpClient(ar);
            IsCLientConnected = true;
            IPEndPoint endPoint = (IPEndPoint)Client.Client.RemoteEndPoint;
            var ipAddress = endPoint.Address;
            Client.ReceiveBufferSize = BufferSize;
            Client.SendBufferSize = BufferSize;
            Debug.WriteLine(ipAddress + " is connected");
            OnClientConnected(ipAddress.ToString());
        }
        catch
        {

        }
    }
    public void CloseServer()
    {
        try
        {
            if (Client != null)
            {
                Client.Close();
                Client = null;
            }
            IsCLientConnected = false;
            IsServerStarted = false;
            if (Listener == null)
                return;
            Listener.Stop();
            Listener = null;
        }
        catch (Exception e)
        {
            Debug.WriteLine("Failed to Stop server! : " + e.ToString());
        }
    }
    public bool SendDataToClient(byte[] data)
    {
        Debug.WriteLine("Function:: " + data[0] + " len: " + data.Length);
        bool success = false;
        byte[] headerBytes = PrepareDataHeader(data.Length);
        int DataLength = headerBytes.Length + data.Length;
        byte[] dataToSend = new byte[DataLength];
        headerBytes.CopyTo(dataToSend, 0);
        data.CopyTo(dataToSend, headerBytes.Length);
        try
        {
            if (Client == null)
                return false;
            if (Client.Connected)
            {
                NetworkStream stream = Client.GetStream();
                if (DataLength < BufferSize)
                {
                    stream.Write(dataToSend, 0, DataLength);
                    success = true;
                }
                else
                {
                    int NumBytesLeft = DataLength;
                    int TotalBytesSent = 0;
                    byte[] tempData;
                    int len = BufferSize;
                    while (NumBytesLeft > 0)
                    {
                        tempData = new byte[len];
                        Array.Copy(dataToSend, TotalBytesSent, tempData, 0, len);
                        stream.Write(tempData, 0, len);
                        NumBytesLeft -= len;
                        TotalBytesSent += len;
                        len = Math.Min(NumBytesLeft, BufferSize);
                    }
                    success = true;
                }
            }
            else
                success = false;
            return success;
        }
        catch (Exception e)
        {
            Debug.WriteLine("Unable to send message to client!" + e.ToString());
            IsCLientConnected = false;
            if (Client == null)
                return false;
            Client.Close();
            Client = null;
            //var t = Task.Run(() => StartListener());
            return false;
        }
    }
    public byte[] GetData()
    {
        try
        {
            if (Client == null)
            {
                return null;
            }

            NetworkStream stream = Client.GetStream();
            byte[] tempData = new byte[BufferSize];
            byte[] dataHeader = new byte[5];
            using (MemoryStream ms = new MemoryStream())
            {
                int numBytesRead = 0;
                int TotalBytesReceived = 0;
                bool isFirstsSampleReceived = false;
                int DataLength = 0;
                while (true)
                {
                    if (!isFirstsSampleReceived)
                    {
                        numBytesRead = stream.Read(dataHeader, 0, dataHeader.Length);
                        if (numBytesRead == dataHeader.Length)
                        {
                            if (dataHeader[0] != StartByte)
                                break;
                            DataLength = BitConverter.ToInt32(dataHeader, 1);
                            isFirstsSampleReceived = true;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (DataLength < BufferSize)
                        {
                            numBytesRead = stream.Read(tempData, 0, DataLength);
                            TotalBytesReceived += numBytesRead;
                            ms.Write(tempData, 0, numBytesRead);
                        }
                        else
                        {
                            int len = BufferSize;
                            while (TotalBytesReceived < DataLength)
                            {
                                numBytesRead = stream.Read(tempData, 0, len);
                                TotalBytesReceived += numBytesRead;
                                ms.Write(tempData, 0, numBytesRead);
                                len = Math.Min(DataLength - TotalBytesReceived, BufferSize);
                            }
                        }
                    }
                    if (TotalBytesReceived >= DataLength)
                        break;
                }
                if (TotalBytesReceived == DataLength)
                {
                    byte[] receivedData = ms.ToArray();
                    return receivedData;
                }
                else
                {
                    Debug.WriteLine("number of received bytes are incorrect: TotalBytesReceived: " + TotalBytesReceived + " DataLength: " + DataLength + " First byte::" + ms.ToArray()[0] + " second first byte:" + ms.ToArray()[DataLength]);
                    return null;
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine("Failed to Receive Data From Client! :" + e.ToString());
            IsCLientConnected = false;
            if (Client != null)
            {
                Client.Close();
                Client = null;
            }
            //var t = Task.Run(() => StartListener());
            return null;
        }
    }
    private byte[] PrepareDataHeader(int len)
    {
        byte[] header = new byte[5];
        header[0] = StartByte;
        byte[] lengthBytes = BitConverter.GetBytes(len);
        lengthBytes.CopyTo(header, 1);
        return header;
    }
    private IPAddress[] GetAllInternetworkIPs()
    {
        List<IPAddress> addressList = new List<IPAddress>();
        var interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
        foreach (var i in interfaces)
            foreach (var ua in i.GetIPProperties().UnicastAddresses)
            {
                if (ua.Address.AddressFamily == AddressFamily.InterNetwork && i.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up && i.Name.Equals("Wi-Fi"))
                {
                    addressList.Add(ua.Address);
                    Debug.WriteLine("name: " + i.Name + " ip: " + ua.Address + "  type: ");
                }
            }
        return addressList.ToArray();
    }
}
