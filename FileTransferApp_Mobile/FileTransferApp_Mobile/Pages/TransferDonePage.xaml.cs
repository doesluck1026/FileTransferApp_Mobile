using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferDonePage : ContentPage
    {
        public TransferDonePage()
        {
            InitializeComponent();
        }

        private async void btn_MainMenu_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PopModalAsync();
            await Navigation.PushModalAsync(new MainPage());
        }

        private async void btn_OpenFolder_Clicked(object sender, EventArgs e)
        {
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(Main.TransferMetrics.CurrentFile.FilePath + Main.TransferMetrics.CurrentFile.FileName)
            });
        }
    }
}