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

    public static bool IsFirstTime { get; set; }

    private static bool didInit = false;
    /// <summary>
    /// Inits parameters and assigns parameters to variables above
    /// </summary>
    public static void Init()
    {
        if (Application.Current.Properties.ContainsKey("DeviceName") == false)
            Application.Current.Properties.Add("DeviceName", DeviceInfo.Name);
        if(Application.Current.Properties.ContainsKey("FirstTime")==false)
            Application.Current.Properties.Add("FirstTime", true);
        didInit = true;
        DeviceName = (string)Application.Current.Properties["DeviceName"];
        IsFirstTime = (bool)Application.Current.Properties["FirstTime"];
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
        Application.Current.Properties["FirstTime"] = IsFirstTime;
    }
}
