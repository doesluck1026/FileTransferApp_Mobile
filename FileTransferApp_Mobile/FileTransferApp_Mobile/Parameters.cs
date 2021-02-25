using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

class Parameters
{
    /// <summary>
    /// User defined name of the device that is shown during scan process
    /// </summary>
    public static string DeviceName { get; set; }  
    /// <summary>
    /// Total size of files transfered via this app since download
    /// </summary>
    public static string TotalTransferedDataSize { get; set; }
    /// <summary>
    /// Total number of files transfered via this app since download
    /// </summary>
    public static int NumberOfFilesTransfered { get; set; }

    private static bool didInit = false;
    /// <summary>
    /// Inits parameters and assigns parameters to variables above
    /// </summary>
    public static void Init()
    {
        if (Application.Current.Properties.ContainsKey("DeviceName") == false)
            Application.Current.Properties.Add("DeviceName", DeviceInfo.Name);
        if (Application.Current.Properties.ContainsKey("TransferSize") == false)
            Application.Current.Properties.Add("TransferSize", "1 Byte");
        if (Application.Current.Properties.ContainsKey("NumFiles") == false)
            Application.Current.Properties.Add("NumFiles", 0);
        didInit = true;
        DeviceName = (string)Application.Current.Properties["DeviceName"];
        TotalTransferedDataSize = (string)Application.Current.Properties["TransferSize"];
        NumberOfFilesTransfered = (int)Application.Current.Properties["NumFiles"];
        didInit = true;
    }
    /// <summary>
    /// Saves Parameters to device memory which are assigned above
    /// </summary>
    public static void Save()
    {
        if (!didInit)
            return;
        Debug.WriteLine("Saving Parameters..");
        Application.Current.Properties["DeviceName"] = DeviceName;
        Application.Current.Properties["TransferSize"] = TotalTransferedDataSize;
        Application.Current.Properties["NumFiles"] = NumberOfFilesTransfered;
    }
}
