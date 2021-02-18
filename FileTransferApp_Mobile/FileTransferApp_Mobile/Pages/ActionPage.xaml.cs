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
    public partial class ActionPage : ContentPage
    {
        public ActionPage()
        {
            InitializeComponent();
        }

        private async void btn_Send_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync((new MainPage()));
        }

        private async void btn_Receive_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync((new ReceivingPage()));

        }
        private async void btn_Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync((new SettingsPage()));
        }

    }
}