using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();

            var list = new List<string>
            {
                "Welcome to B.C. File Transfer ",
                "You will learn how to use it",
                "It is an useful and time-saving app,",
                "also very easy to use."
            };
            TheCarousel.ItemsSource = list;

        }

        private void btn_youtube_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://www.youtube.com/channel/UCkUWRx8ozzEgi7R2I6OY-BQ");
        }

        private void btn_insta_Clicked(object sender, EventArgs e)
        {

        }

        private void btn_github_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://github.com/doesluck1026");
        }

        private void btn_patreon_Clicked(object sender, EventArgs e)
        {

        }
    }
}