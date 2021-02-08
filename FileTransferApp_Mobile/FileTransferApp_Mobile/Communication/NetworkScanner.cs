using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Rssdp;

class NetworkScanner
{
    public delegate void ScanCompleteDelegate(string IPandHostName);
    public  event ScanCompleteDelegate OnScanCompleted;

    private int IPend;
    private static SsdpDevicePublisher _Publisher;

    private static string IP;

    public NetworkScanner(int ipend)
    {
        IPend = ipend;
    }
    public void ScanAvailableDevices()
    {
        //string gate_ip= "192.168.1.1";
        //string[] array = gate_ip.Split('.');
        //    string ping_var = array[0] + "." + array[1] + "." + array[2] + "." + IPend;
        ////if(ping_var!= thisIP)
        //    Ping(ping_var, 1, 100);
        
        //OnScanCompleted(DeviceList.ToArray());

    }
    // Call this method from somewhere to actually do the publish.
    public static void PublishDevice()
    {
        // As this is a sample, we are only setting the minimum required properties.
        var deviceDefinition = new SsdpRootDevice()
        {
            CacheLifetime = TimeSpan.FromMinutes(30), //How long SSDP clients can cache this info.
            Location = new Uri("http://mydevice/descriptiondocument.xml"), // Must point to the URL that serves your devices UPnP description document. 
            DeviceTypeNamespace = "da",
            DeviceType = Xamarin.Essentials.DeviceInfo.DeviceType.ToString(),
            FriendlyName = Xamarin.Essentials.DeviceInfo.Model,
            Manufacturer = Xamarin.Essentials.DeviceInfo.Manufacturer,
            ModelName = Xamarin.Essentials.DeviceInfo.Name,
            Uuid = Xamarin.Essentials.DeviceInfo.Idiom.ToString() // This must be a globally unique value that survives reboots etc. Get from storage or embedded hardware etc.
        };
        _Publisher = new SsdpDevicePublisher();
        _Publisher.AddDevice(deviceDefinition);
    }
    private static SsdpDeviceLocator _DeviceLocator;

    // Call this method from somewhere in your code to start the search.
    public static void BeginSearch()
    {
        _DeviceLocator = new SsdpDeviceLocator();

        // (Optional) Set the filter so we only see notifications for devices we care about 
        // (can be any search target value i.e device type, uuid value etc - any value that appears in the 
        // DiscoverdSsdpDevice.NotificationType property or that is used with the searchTarget parameter of the Search method).
        _DeviceLocator.NotificationFilter = "";

        // Connect our event handler so we process devices as they are found
        _DeviceLocator.DeviceAvailable += deviceLocator_DeviceAvailable;

        // Enable listening for notifications (optional)
        _DeviceLocator.StartListeningForNotifications();

        // Perform a search so we don't have to wait for devices to broadcast notifications 
        // again to get any results right away (notifications are broadcast periodically).
       // _DeviceLocator.SearchAsync();
    }

    // Process each found device in the event handler
    async static void deviceLocator_DeviceAvailable(object sender, DeviceAvailableEventArgs e)
    {
        //Device data returned only contains basic device details and location of full device description.
        Debug.WriteLine("Found " + e.DiscoveredDevice.Usn + " at " + e.DiscoveredDevice.DescriptionLocation.ToString());

        //Can retrieve the full device description easily though.
        var fullDevice = await e.DiscoveredDevice.GetDeviceInfo();
        Debug.WriteLine("name: "+fullDevice.FriendlyName);
    }
    public static void GetDeviceAddress(out string deviceIP,out string deviceHostname)
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
        IP = localAddr.ToString();
    }

    private  void Ping(string host, int attempts, int timeout)
    {
        for (int i = 0; i < attempts; i++)
        {
            new Thread(delegate ()
            {
                try
                {
                    System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                    ping.PingCompleted += new PingCompletedEventHandler(PingCompleted);
                    ping.SendAsync(host, timeout, host);
                    Debug.WriteLine("Hostname: " + Dns.GetHostEntry(host).HostName);
                }
                catch
                {
                        // Do nothing and let it try again until the attempts are exausted.
                        // Exceptions are thrown for normal ping failurs like address lookup
                        // failed.  For this reason we are supressing errors.
                }
            }).Start();
        }
    }
    private  void PingCompleted(object sender, PingCompletedEventArgs e)
    {
        string ip = (string)e.UserState;
        if (e.Reply != null && e.Reply.Status == IPStatus.Success)
        {
            string hostname = Dns.GetHostEntry(ip).HostName;

            Debug.WriteLine("found device: " + ip + "hotname: " + hostname) ;
            OnScanCompleted(hostname);
        }
        else
        {
            // MessageBox.Show(e.Reply.Status.ToString());
        }
    }
    public  string GetHostName(string ipAddress)
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    public  string GetMacAddress(string ipAddress)
    {
        string macAddress = string.Empty;
        System.Diagnostics.Process Process = new System.Diagnostics.Process();
        Process.StartInfo.FileName = "arp";
        Process.StartInfo.Arguments = "-a " + ipAddress;
        Process.StartInfo.UseShellExecute = false;
        Process.StartInfo.RedirectStandardOutput = true;
        Process.StartInfo.CreateNoWindow = true;
        Process.Start();
        string strOutput = Process.StandardOutput.ReadToEnd();
        string[] substrings = strOutput.Split('-');
        if (substrings.Length >= 8)
        {
            macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                     + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                     + "-" + substrings[7] + "-"
                     + substrings[8].Substring(0, 2);
            return macAddress;
        }
        else
        {
            return "OWN Machine";
        }
    }
    /// <summary>
    /// Gets current device's hostname.
    /// </summary>
    /// <returns>ip as string</returns>
    public  string GetDeviceHostName()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        return host.HostName;
    }
}
 