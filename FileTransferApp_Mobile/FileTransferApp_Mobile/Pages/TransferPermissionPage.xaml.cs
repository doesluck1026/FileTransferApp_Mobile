using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.TransferSize = transferSize;
            this.SenderDevice = senderName;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lbl_TransferInfo.Text = SenderDevice + " wants to send you files: " + TransferSize + " \n Do you want to receive?";
        }
        private async void btn_Accept_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TransferPage());
            Main.ResponseToTransferRequest(true);
        }

        private async void btn_Reject_Clicked(object sender, EventArgs e)
        {
            Main.ResponseToTransferRequest(false);
            await Navigation.PushAsync(new MainPage());
        }
    }
}