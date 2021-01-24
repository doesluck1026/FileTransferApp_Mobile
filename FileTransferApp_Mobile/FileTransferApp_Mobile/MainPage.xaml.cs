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
        private Main main;
        public MainPage()
        {
            InitializeComponent();
            Instance = this;
            main = new Main();
            main.StartServer();
            NetworkScanner.GetDeviceAddress(out DeviceIP, out DeviceHostName);
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                lbl_IP.Text = DeviceIP;
                lbl_HostName.Text = DeviceHostName;
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
                main.SetFilePaths(filepaths);
            }
        }

        private async void btn_SelectFile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SendingPage());
        }
        private void btn_Scan_Clicked(object sender, EventArgs e)
        {
            ScanNetwork();
        }
        private async void ScanNetwork()
        {
            await Task.Run(() => PartialScan(2, 100));
        }
        private void PartialScan(int startx,int endx)
        {
            for(int i = startx; i < endx; i++)
            {
                try
                {
                    PingADevice(i);
                    Debug.WriteLine("pinged: " + i);
                }
                catch
                {
                    Debug.WriteLine("failed: " + i);

                }
            }
        }
        private void PingADevice(int ipend)
        {
            var dummyDevice = new NetworkScanner(ipend);
            dummyDevice.OnScanCompleted += DummyDevice_OnScanCompleted;
            dummyDevice.ScanAvailableDevices();
        }
        private void DummyDevice_OnScanCompleted(string IPandHostName)
        {
            if (!AvailableDeviceList.Contains(IPandHostName))
                AvailableDeviceList.Add(IPandHostName);
        }
    }
}
