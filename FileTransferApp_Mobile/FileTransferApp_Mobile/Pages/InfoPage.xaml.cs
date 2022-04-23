using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace FileTransferApp_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();
            
            //if (!Admob.TestMode)
            //    BannerView.AdsId = Admob.BannerAdID;
        }
        protected override void OnAppearing()
        {
            //Admob.AdjustBannerView(BannerView);
            Main.OnClientRequested += Main_OnClientRequested;
            
        }
        protected override void OnDisappearing()
        {
            Main.OnClientRequested -= Main_OnClientRequested;
        }
        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Pages.ActionPage());
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
        private void btn_youtube_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://www.youtube.com/channel/UCkUWRx8ozzEgi7R2I6OY-BQ");
        }

        private void btn_insta_Clicked(object sender, EventArgs e)
        {

        }

        private void btn_blogger_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://buggycompany.blogspot.com");
        }

        private void btn_patreon_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://www.patreon.com/buggycompany");
        }

        private void btn_Settings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Pages.SettingsPage());
        }

        private void btn_Info_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Pages.ActionPage());
        }
    }
}