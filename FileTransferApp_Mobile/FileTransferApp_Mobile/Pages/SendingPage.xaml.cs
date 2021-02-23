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
        public string[] AvailableDeviceArray;
        private string TargetDeviceIP;
        public SendingPage()
        {
            InitializeComponent();
            Instance = this;
            NetworkScanner.OnScanCompleted += NetworkScanner_OnScanCompleted;
        }

        private void NetworkScanner_OnScanCompleted()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (NetworkScanner.DeviceNames != null)
                {
                    AvailableDeviceArray = NetworkScanner.DeviceNames.ToArray();
                    list_Devices.ItemsSource = AvailableDeviceArray;
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (NetworkScanner.DeviceNames!= null)
            {
                AvailableDeviceArray = NetworkScanner.DeviceNames.ToArray();
                list_Devices.ItemsSource = AvailableDeviceArray;
            }
        }
        private async void btn_SendFile_Clicked(object sender, EventArgs e)
        {
            bool didDeviceAccept = Main.ConnectToTargetDevice(txt_ClientIP.Text);
            Debug.WriteLine("Receiver Response: " + didDeviceAccept);
            if (didDeviceAccept)
            {
                Main.BeginSendingFiles();
                await Navigation.PushModalAsync(new TransferPage());
            }
        }

        private void list_Devices_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            TargetDeviceIP = NetworkScanner.DeviceIPs[NetworkScanner.DeviceNames.IndexOf(list_Devices.SelectedItem.ToString())];
            txt_ClientIP.Text = TargetDeviceIP;//list_Clients.SelectedItem.ToString();
        }

        private void btn_ScanDevices_Clicked(object sender, EventArgs e)
        {
            NetworkScanner.DeviceNames = new List<string>();
            NetworkScanner.ScanAvailableDevices();
        }
    }
}