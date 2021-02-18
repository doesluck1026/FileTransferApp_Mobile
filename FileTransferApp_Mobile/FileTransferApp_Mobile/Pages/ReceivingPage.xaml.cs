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
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            using (var progress = Acr.UserDialogs.UserDialogs.Instance.Loading("Waiting..."))
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