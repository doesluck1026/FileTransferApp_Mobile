using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

    public static string DeviceLanguage { get; set; }

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
        if (Application.Current.Properties.ContainsKey("Language") == false)
        {
            char[] splitter = { '-' };
            string language = CultureInfo.InstalledUICulture.ToString().Split(splitter)[0];
            Application.Current.Properties.Add("Language", language);
            Debug.WriteLine("culture lang: " + language);
        }
        didInit = true;
        DeviceName = (string)Application.Current.Properties["DeviceName"];
        IsFirstTime = (bool)Application.Current.Properties["FirstTime"];
        DeviceLanguage=(string)Application.Current.Properties["Language"];
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
        Application.Current.Properties["Language"] = DeviceLanguage;
    }
}
