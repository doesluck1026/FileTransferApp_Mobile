using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            // First display Spalsh screen
            MainPage = new NavigationPage(new SplashScreen());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
