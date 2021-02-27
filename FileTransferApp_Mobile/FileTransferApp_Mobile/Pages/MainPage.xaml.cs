using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;


namespace FileTransferApp_Mobile
{
    public partial class MainPage : ContentPage
    {
        public List<string> AvailableDeviceList=new List<string>();
        public static MainPage Instance;
        private string DeviceIP;
        private string DeviceHostName;
        public MainPage()
        {
            InitializeComponent();
            Instance = this;
            DeviceDisplay.KeepScreenOn = false;
            Main.OnClientRequested += Main_OnClientRequested;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NetworkScanner.GetDeviceAddress(out DeviceIP, out DeviceHostName);
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                lbl_IP.Text = DeviceIP;
                lbl_HostName.Text = Parameters.DeviceName;
            });
        }
        private void Main_OnClientRequested(string totalTransferSize, string deviceName)
        {
            /// Show file transfer request and ask for permission here
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new TransferPermissionPage(totalTransferSize, deviceName));
            });
        }
        /// <summary>
        /// The address of the file to be processed is selected
        /// </summary>
        /// <returns>the address of the file in memory</returns>
        private async void SelectFile()
        {
            var pickResult = await FilePicker.PickMultipleAsync();
            if (pickResult != null)
            {
                var results = pickResult.ToArray();
                string[] filepaths = new string[results.Length];
                for(int i=0;i<filepaths.Length;i++)
                {
                    filepaths[i] = results[i].FullPath;
                }
                Main.SetFilePaths(filepaths);
                await Navigation.PushAsync(new Pages.FilesPage());
            }
        }

        private void btn_SelectFile_Clicked(object sender, EventArgs e)
        {
            SelectFile();
        }
    }
}
