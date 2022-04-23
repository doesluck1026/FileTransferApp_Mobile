using FileTransferApp_Mobile.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using Xamarin.Essentials;

namespace FileTransferApp_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        Dictionary<string, string> LanguageList = new Dictionary<string, string>();
        public SettingsPage()
        {
            InitializeComponent();
            //if (!Admob.TestMode)
            //    BannerView.AdsId = Admob.BannerAdID;
            LanguageList.Add("English", "en");
            LanguageList.Add("Türkçe", "tr");
            LanguageList.Add("Deutsche", "de");
            LanguageList.Add("Française", "fr");
            LanguageList.Add("Española", "es");
            LanguageList.Add("中国人", "zh-Hans");
            LanguageList.Add("عربي", "ar");
        }
        
        protected override void OnAppearing()
        {
            //Admob.AdjustBannerView(BannerView);
            Main.OnClientRequested += Main_OnClientRequested;
            Device.InvokeOnMainThreadAsync(() =>
            {
                if (Parameters.IsFirstTime)
                    Frame_firstTimeInfo.IsVisible = true;
                else
                    Frame_firstTimeInfo.IsVisible = false;
                txt_DeviceName.Text = Parameters.DeviceName;
                var languageCodeList = LanguageList.Values.ToList();
                Picker_Languages.ItemsSource = LanguageList.Keys.ToArray();
                Picker_Languages.SelectedIndex = languageCodeList.IndexOf(Parameters.DeviceLanguage);
            });
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
        private async void btn_Save_Clicked(object sender, EventArgs e)
        {
            Parameters.DeviceName = txt_DeviceName.Text;
            string selectedLanguage = Picker_Languages.SelectedItem.ToString();
            string languageCode;
            LanguageList.TryGetValue(selectedLanguage, out languageCode);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
            AppResources.Culture = new CultureInfo(languageCode);
            Parameters.DeviceLanguage = languageCode;
            Parameters.Save();
            using (var progress = Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.Setting_Warning_Saving))
            {
                for (var i = 0; i < 100; i++)
                {
                    //progress.PercentComplete = i;
                    await Task.Delay(5);
                }
            }
            await Navigation.PushAsync(new Pages.ActionPage());         /// Close this page.
        }

        private void btn_Settings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Pages.ActionPage());
        }

        private void btn_Info_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Pages.InfoPage());
        }

        private void btn_GetDesktopApp_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://buggycompany.blogspot.com/2021/03/buggy-file-transfer.html");
        }
    }
}