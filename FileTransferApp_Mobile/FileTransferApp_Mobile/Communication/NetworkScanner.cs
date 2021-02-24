﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;

class NetworkScanner
{
    public delegate void ScanCompleteDelegate();
    public static event ScanCompleteDelegate OnScanCompleted;
        
    private static string DeviceIP;             /// ip of this device
    private static readonly int PublishPort = 42019;
    private static Server publisherServer;
    private static Client client;
    public static List<string> DeviceNames = new List<string>();
    public static List<string> DeviceIPs = new List<string>();
    public static void ScanAvailableDevices()
    {
        Thread.Sleep(500);
        string deviceIP, deviceHostname;
        GetDeviceAddress(out deviceIP, out deviceHostname);
        DeviceIP = deviceIP;
        DeviceNames.Clear();
        DeviceIPs.Clear();
        char[] splitter = new char[] { '.' };
        var ipStack = deviceIP.Split(splitter);
        string ipHeader = "";
        for (int i = 0; i < 3; i++)
        {
            ipHeader += ipStack[i] + ".";
        }
        IPHolder holder = new IPHolder(2, 256, ipHeader);
        Thread t = new Thread(new ParameterizedThreadStart(ParallelScan));
        t.Start(holder);

    }

    private static void ParallelScan(object sender)
    {
        Stopwatch stp = Stopwatch.StartNew();
        IPHolder holder = (IPHolder)sender;
        Debug.WriteLine("Buradaydı");
        for (int i = holder.StartIP; i < holder.StopIP; i++)
        {
            try
            {
                //Debug.WriteLine("Pinging: " + holder.IpHeader + i.ToString());
                GetDeviceData(holder.IpHeader + i.ToString());
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        Debug.WriteLine("scanning time: " + stp.Elapsed.TotalSeconds + " s");

        if (OnScanCompleted != null)
            OnScanCompleted.Invoke();
    }



    private static void GetDeviceData(string IP)
    {
        //Stopwatch stp = Stopwatch.StartNew();
        client = new Client(port: PublishPort, ip: IP);
        string clientIP = client.ConnectToServer(25);
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
        publisherServer.OnClientConnected += PublisherServer_OnClientConnected;
        publisherServer.StartListener();
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

    private class IPHolder
    {
        public int StartIP;
        public int StopIP;
        public string IpHeader;
        public IPHolder(int startIP, int stopIP, string ipHeader)
        {
            this.StartIP = startIP;
            this.StopIP = stopIP;
            this.IpHeader = ipHeader;
        }
    }
}
