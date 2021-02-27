using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferDonePage : ContentPage
    {
        private int selectedFileIndex =0;
        public TransferDonePage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            list_Files.ItemsSource = Main.FileNames;
            list_Files.SelectedItem = Main.FileNames[selectedFileIndex];
        }
        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Pages.ActionPage());
            return true;
        }
        private async void btn_MainMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.ActionPage());
        }

        private async void btn_OpenFolder_Clicked(object sender, EventArgs e)
        {
            string selectedFilePath = Main.FilePaths[selectedFileIndex];
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(selectedFilePath)
            });
        }
        private void list_Files_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedFileIndex = Main.FileNames.ToList().IndexOf(list_Files.SelectedItem.ToString());
        }
    }
}