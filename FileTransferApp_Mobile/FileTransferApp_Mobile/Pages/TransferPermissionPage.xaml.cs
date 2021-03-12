using System;
using FileTransferApp_Mobile.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferPermissionPage : ContentPage
    {
        private string TransferSize;
        private string SenderDevice;
        public TransferPermissionPage(string transferSize,string senderName)
        {
            InitializeComponent();
            if (!Admob.TestMode)
                BannerView.AdsId = Admob.BannerAdID;
            this.TransferSize = transferSize;
            this.SenderDevice = senderName;
        }
        protected override void OnAppearing()
        {
            Admob.AdjustBannerView(BannerView);
            
            lbl_TransferInfo.Text = SenderDevice + AppResources.Permission_RequestMessage + TransferSize + " \n "+AppResources.Permission_RequestMessage2;
        }
        private async void btn_Accept_Clicked(object sender, EventArgs e)
        {
            Main.IsSending = false;
            await Navigation.PushAsync(new TransferPage());
        }

        private async void btn_Reject_Clicked(object sender, EventArgs e)
        {
            Main.ResponseToTransferRequest(false);
            await Navigation.PushAsync(new Pages.ActionPage());
        }
    }
}