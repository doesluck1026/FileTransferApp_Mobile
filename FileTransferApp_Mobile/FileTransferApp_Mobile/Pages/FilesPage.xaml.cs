using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FileTransfer;
using FileTransfer.Communication;

namespace FileTransferApp_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilesPage : ContentPage
    {
        private List<string> FilePaths;
        private string[] FileNames;
        private int SelectedIndex;
        public FilesPage()
        {
            InitializeComponent();
            FilePaths = TransferEngine.FilePaths.ToList();
            SelectedIndex = 0;
            //if (!Admob.TestMode)
            //    BannerView.AdsId = Admob.BannerAdID;
        }
        protected override void OnAppearing()
        {
            //Admob.AdjustBannerView(BannerView);
            ShowSelectedFiles();
            TransferEngine.OnClientRequested += Main_OnClientRequested;

        }

        protected override void OnDisappearing()
        {
            TransferEngine.OnClientRequested -= Main_OnClientRequested;
        }

        private void Main_OnClientRequested(string totalTransferSize, string deviceName, bool isAlreadyAccepted)
        {
            /// Show file transfer request and ask for permission here
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new TransferPermissionPage(totalTransferSize, deviceName));
            });
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
                    if (!FilePaths.Contains(results[i].FullPath))
                        FilePaths.Add(results[i].FullPath);
                }
                ShowSelectedFiles();
            }
        }
        private void btn_Add_Clicked(object sender, EventArgs e)
        {
            SelectFile();
        }

        private async void btn_Open_Clicked(object sender, EventArgs e)
        {
            if (FilePaths==null || FilePaths.Count == 0)
                return;
            string selectedFilePath = FilePaths[SelectedIndex];
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

            ShowSelectedFiles();
        }
        private void ShowSelectedFiles()
        {
            if (FilePaths == null || FilePaths.Count == 0)
                return;
            FileNames = new string[FilePaths.Count];
            for (int i = 0; i < FilePaths.Count; i++)
            {
                var filePathParts= FilePaths[i].Split(new char[] { '/' });
                FileNames[i] = filePathParts[filePathParts.Length-1];
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                list_Files.ItemsSource = FileNames;
                SelectedIndex = Math.Min(SelectedIndex, FilePaths.Count - 1);
                list_Files.SelectedItem = FileNames[SelectedIndex];
            });
        }
        private async void btn_Send_Clicked(object sender, EventArgs e)
        {
            if(FilePaths.Count>0)
            {
                TransferEngine.SetFilePaths(FilePaths.ToArray());
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
            SelectedIndex = FileNames.ToList().IndexOf(list_Files.SelectedItem.ToString());
        }
    }
}