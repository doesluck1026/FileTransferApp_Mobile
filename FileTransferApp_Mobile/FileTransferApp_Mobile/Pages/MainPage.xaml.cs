using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;


namespace FileTransferApp_Mobile
{
    public partial class MainPage : ContentPage
    {
        private string DeviceIP;
        private string DeviceHostName;
        public MainPage()
        {
            InitializeComponent();
            DeviceDisplay.KeepScreenOn = false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Main.OnClientRequested += Main_OnClientRequested;
            NetworkScanner.GetDeviceAddress(out DeviceIP, out DeviceHostName);
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                lbl_IP.Text = DeviceIP;
                lbl_HostName.Text = Parameters.DeviceName;
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Main.OnClientRequested -= Main_OnClientRequested;
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new Pages.ActionPage());
            });
            return true;
        }
        /// <summary>
        /// This event occurs when a device connected to this device to send som files.
        /// </summary>
        /// <param name="totalTransferSize">Total size of transfer as the unit of byte</param>
        /// <param name="deviceName">Name of the device which has connected to this device.</param>
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
