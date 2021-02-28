using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class SendingPage : ContentPage
    {
        private string TargetDeviceIP;
        private string TargetDeviceName;
        private bool isRequestSent = false;
        public SendingPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            list_Devices.SeparatorColor = Color.AliceBlue;
            list_Devices.RefreshControlColor = Color.Black;

            NetworkScanner.OnScanCompleted += NetworkScanner_OnScanCompleted;
            Main.OnTransferResponded += Main_OnTransferResponded;
            if (NetworkScanner.DeviceNames != null)
            {
                if (NetworkScanner.DeviceNames.Count > 0)
                {
                    list_Devices.ItemsSource = NetworkScanner.DeviceNames.ToArray();
                    list_Devices.SelectedItem = NetworkScanner.DeviceNames[0];
                    list_Devices_ItemSelected(null, null);
                }
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            NetworkScanner.OnScanCompleted -= NetworkScanner_OnScanCompleted;
            Main.OnTransferResponded -= Main_OnTransferResponded;
        }
        protected override bool OnBackButtonPressed()
        {
            if(!isRequestSent)
                return base.OnBackButtonPressed();
            else
            {
                using (Acr.UserDialogs.UserDialogs.Instance.Alert("You can't go back after sending a request. Sorry..."))
                {
                    Task.Delay(200);
                }
                return true;
            }
        }
        private void Main_OnTransferResponded(bool isAccepted)
        {
            Debug.WriteLine("Receiver Response: " + isAccepted);
            isRequestSent = false;
            if (isAccepted)
            {
                Device.InvokeOnMainThreadAsync(() =>
                {
                    Navigation.PushAsync(new TransferPage());
                });
                Main.BeginSendingFiles();
            }
            else
            {
                using (Acr.UserDialogs.UserDialogs.Instance.Alert("Transfer request is rejected by "+ TargetDeviceName + ". Sorry..."))
                {
                    Task.Delay(200);
                }
            }
        }

        private void NetworkScanner_OnScanCompleted()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (NetworkScanner.DeviceNames != null)
                {
                    if(NetworkScanner.DeviceNames.Count>0)
                        list_Devices.ItemsSource = NetworkScanner.DeviceNames.ToArray();
                }
            });
        }

       
        private void btn_SendFile_Clicked(object sender, EventArgs e)
        {
            if (!isRequestSent)
            {
                isRequestSent = true;
                Main.ConnectToTargetDevice(txt_ClientIP.Text);
                if(NetworkScanner.DeviceNames.Count>0)
                {
                    int index = NetworkScanner.DeviceIPs.IndexOf(txt_ClientIP.Text.ToString());
                    if(index>=0&& index< NetworkScanner.DeviceNames.Count)
                        TargetDeviceName = NetworkScanner.DeviceNames[index];
                }
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
            Task.Run(() =>
            {
                Thread.Sleep(500);
                using (var progress = Acr.UserDialogs.UserDialogs.Instance.Progress("Scanning Network..."))
                {
                    while (NetworkScanner.IsScanning)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (NetworkScanner.DeviceNames != null)
                                if (NetworkScanner.DeviceNames.Count > 0)
                                    list_Devices.ItemsSource = NetworkScanner.DeviceNames.ToArray();

                            progress.PercentComplete = NetworkScanner.ScanPercentage;
                        });
                        Thread.Sleep(100);
                    }
                }
            });
        }
    }
}