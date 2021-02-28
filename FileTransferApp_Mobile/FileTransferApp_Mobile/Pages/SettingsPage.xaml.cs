using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Main.OnClientRequested += Main_OnClientRequested;
            Device.InvokeOnMainThreadAsync(() =>
            {
                if (Parameters.IsFirstTime)
                    Frame_firstTimeInfo.IsVisible = true;
                else
                    Frame_firstTimeInfo.IsVisible = false;
                txt_DeviceName.Text = Parameters.DeviceName;
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
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
            Parameters.Save();
            using (var progress = Acr.UserDialogs.UserDialogs.Instance.Loading("Saving Parameters"))
            {
                for (var i = 0; i < 100; i++)
                {
                    //progress.PercentComplete = i;
                    await Task.Delay(5);
                }
            }
            Navigation.PushAsync(new Pages.ActionPage());         /// Close this page.
        }

        private void btn_Settings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Pages.ActionPage());
        }

        private void btn_Info_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Pages.InfoPage());
        }
    }
}