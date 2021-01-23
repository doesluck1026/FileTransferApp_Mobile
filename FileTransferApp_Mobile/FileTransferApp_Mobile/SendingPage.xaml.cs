using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
   
    public partial class SendingPage : ContentPage
    {
        public static SendingPage Instance;
        public string[] AvailableDeviceList ;
        public SendingPage()
        {
            InitializeComponent();
            Instance = this;
            NetworkScanner.OnScanCompleted += NetworkScanner_OnScanCompleted;

        }

        private void btn_SendFile_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Buton basıldı");
            list_Devices.ItemsSource = new string[]
                {
                    "selam",
                    "sana",
                    "demir",
                    "ayak",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                };
        }
        private void NetworkScanner_OnScanCompleted(string[] devices)
        {
            Debug.WriteLine("Ping Compeleted!");
            AvailableDeviceList = new string[devices.Length];
            devices.CopyTo(AvailableDeviceList, 0);
            list_Devices.ItemsSource = AvailableDeviceList;

        }
    }
}