using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;


namespace FileTransferApp_Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btn_SelectFile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SendingPage());
            //await Task.Run(() => NetworkScanner.ScanAvailableDevices());
            
        }
   
    }
}
