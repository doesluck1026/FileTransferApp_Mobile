using MarcTron.Plugin;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FileTransfer;
using FileTransfer.Communication;

namespace FileTransferApp_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferDonePage : ContentPage
    {
        private int selectedFileIndex =0;
        public TransferDonePage()
        {
            InitializeComponent();
            //if (!Admob.TestMode)
            //    BannerView.AdsId = Admob.BannerAdID;
        }
        protected override void OnAppearing()
        {
            TransferEngine.OnClientRequested += Main_OnClientRequested;
            //Admob.AdjustBannerView(BannerView);
            //if (Admob.IsInterstitialLoaded)
            //    Admob.ShowInterstitialAd();
            //else
            //    CrossMTAdmob.Current.OnInterstitialLoaded += Current_OnInterstitialLoaded;
            list_Files.ItemsSource = TransferEngine.FileNames;
            list_Files.SelectedItem = TransferEngine.FileNames[selectedFileIndex];
        }
        protected override void OnDisappearing()
        {
            //CrossMTAdmob.Current.OnInterstitialLoaded -= Current_OnInterstitialLoaded;
            TransferEngine.OnClientRequested -= Main_OnClientRequested;
        }
        private void Current_OnInterstitialLoaded(object sender, EventArgs e)
        {
            //Admob.ShowInterstitialAd();
        }
        private void Main_OnClientRequested(string totalTransferSize, string senderDevice, bool isAlreadyAccepted)
        {
            /// Show file transfer request and ask for permission here
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new TransferPermissionPage(totalTransferSize, senderDevice));
            });
        }
        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Pages.ActionPage());
            return true;
        }
        private async void btn_MainMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.ActionPage());
        }

        private async void btn_OpenFolder_Clicked(object sender, EventArgs e)
        {
            string selectedFilePath = TransferEngine.FilePaths[selectedFileIndex];
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(selectedFilePath)
            });
        }
        private void list_Files_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedFileIndex = TransferEngine.FileNames.ToList().IndexOf(list_Files.SelectedItem.ToString());
        }
    }
}