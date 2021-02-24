using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {

        int SplashScreenDisplayTime = 2000; // in ms

        public SplashPage()
        {
            InitializeComponent();
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Thread.Sleep(SplashScreenDisplayTime); // Simulate a long loading process on app startup.
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new NavigationPage(new ActionPage());
                });
            });
        }

    }
}