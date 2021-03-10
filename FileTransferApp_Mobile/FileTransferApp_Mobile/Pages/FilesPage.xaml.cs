using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilesPage : ContentPage
    {
        private List<string> FilePaths;
        private int SelectedIndex;
        public FilesPage()
        {
            InitializeComponent();
            FilePaths = Main.FilePaths.ToList();
            SelectedIndex = 0;
            if (!Admob.TestMode)
                BannerView.AdsId = Admob.BannerAdID;
        }
        protected override void OnAppearing()
        {
            Admob.AdjustBannerView(BannerView);
            list_Files.ItemsSource = FilePaths;
            list_Files.SelectedItem = FilePaths[SelectedIndex];
        }
        /// <summary>
        /// The address of the file to be processed is selected
        /// </summary>
        /// <returns>the address of the file in memory</returns>
        private async void SelectFile()
        {
            var pickResult = await FilePicker.PickMultipleAsync();
            if (pickResult != null)
            {
                var results = pickResult.ToArray();
                for (int i = 0; i < results.Length; i++)
                {
                    FilePaths.Add(results[i].FullPath);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    list_Files.ItemsSource = FilePaths.ToArray();
                    list_Files.SelectedItem = FilePaths[SelectedIndex];
                });
            }
        }
        private void btn_Add_Clicked(object sender, EventArgs e)
        {
            SelectFile();
        }

        private async void btn_Open_Clicked(object sender, EventArgs e)
        {
            if (FilePaths.Count == 0)
                return;
            string selectedFilePath = Main.FilePaths[SelectedIndex];
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(selectedFilePath)
            });
        }

        private void btn_Rempve_Clicked(object sender, EventArgs e)
        {
            if(FilePaths.Count>0)
                FilePaths.RemoveAt(SelectedIndex);
            if(FilePaths.Count==0)
            {
                SelectedIndex = 0;
                list_Files.ItemsSource = null;
                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                list_Files.ItemsSource = FilePaths.ToArray();
                SelectedIndex = Math.Min(SelectedIndex, FilePaths.Count-1);
                list_Files.SelectedItem = FilePaths[SelectedIndex];
            });
        }

        private async void btn_Send_Clicked(object sender, EventArgs e)
        {
            if(FilePaths.Count>0)
            {
                Main.SetFilePaths(FilePaths.ToArray());
                await Navigation.PushAsync(new SendingPage());
            }
            else
            {
                using (var progress = Acr.UserDialogs.UserDialogs.Instance.Alert("No File is selected!"))
                {
                    for (var i = 0; i < 100; i++)
                    {
                        //progress.PercentComplete = i;
                        await Task.Delay(100);
                    }
                }
            }
        }

        private void list_Files_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            SelectedIndex = FilePaths.ToList().IndexOf(list_Files.SelectedItem.ToString());
        }
    }
}