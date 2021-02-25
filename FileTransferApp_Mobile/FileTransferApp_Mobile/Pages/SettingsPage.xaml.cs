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
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.InvokeOnMainThreadAsync(() =>
            {
                txt_DeviceName.Text = Parameters.DeviceName;
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
                    await Task.Delay(20);
                }
            }
        }
    }
}