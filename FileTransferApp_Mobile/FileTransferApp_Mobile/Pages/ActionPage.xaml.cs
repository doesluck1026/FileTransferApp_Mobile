using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActionPage : ContentPage
    {
        public ActionPage()
        {
            InitializeComponent();
            Main.StartServer();
            if(!NetworkScanner.IsDevicePublished)
                NetworkScanner.PublishDevice();
            NetworkScanner.ScanAvailableDevices();
            Parameters.Init();
            Main.FileSaveURL = GetSaveFilePath();
            Main.OnClientRequested += Main_OnClientRequested;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NetworkScanner.GetDeviceAddress(out string DeviceIP, out string DeviceHostName);
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                lbl_IP.Text = DeviceIP;
                lbl_HostName.Text = Parameters.DeviceName;
            });
        }
        private void Main_OnClientRequested(string totalTransferSize, string senderDevice)
        {
            /// Show file transfer request and ask for permission here
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new TransferPermissionPage(totalTransferSize, senderDevice));
            });
        }

        private async void btn_Send_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync((new MainPage()));
        }

        private async void btn_Receive_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync((new ReceivingPage()));

        }
        private async void btn_Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync((new SettingsPage()));
        }
        private string GetSaveFilePath()
        {
            if (Device.RuntimePlatform == Device.Android)
                return "/storage/emulated/0/Download/";
            else
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/";
        }

        private async void btn_Info_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync((new InfoPage()));

        }
    }
}