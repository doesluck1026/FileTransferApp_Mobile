using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FileTransferApp_Mobile.Resources;
using FileTransfer;
using FileTransfer.Communication;

namespace FileTransferApp_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {

        int SplashScreenDisplayTime = 2000; // in ms

        public SplashPage()
        {
            InitializeComponent();            
        }
        protected override void OnAppearing()
        {
            Parameters.Init();
            TransferEngine.StartServer();
            string deviceIP, deviceHostname;
            NetworkScanner.GetDeviceAddress(out deviceIP, out deviceHostname);
            if (!NetworkScanner.IsDevicePublished)
                NetworkScanner.PublishDevice(Parameters.DeviceName);
            NetworkScanner.ScanAvailableDevices();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Parameters.DeviceLanguage);
            AppResources.Culture = new CultureInfo(Parameters.DeviceLanguage);
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Thread.Sleep(SplashScreenDisplayTime); // Simulate a long loading process on app startup.
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new NavigationPage(new ActionPage());
                });
            });
        }

    }
}