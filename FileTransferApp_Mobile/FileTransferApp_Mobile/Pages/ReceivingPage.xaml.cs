using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivingPage : ContentPage
    {
        public ReceivingPage()
        {
            InitializeComponent();
            Main.OnClientRequested += Main_OnClientRequested;
        }

        private void Main_OnClientRequested(string totalTransferSize, string senderDevice)
        {
            /// Show file transfer request and ask for permission here
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new TransferPermissionPage(totalTransferSize, senderDevice));
            });
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            using (var progress = Acr.UserDialogs.UserDialogs.Instance.Loading("Waiting For Connection..."))
            {
                for (var i = 0; i < 100; i++)
                {
                    //progress.PercentComplete = i;
                    await Task.Delay(20);
                }
            }
        }

    }
}