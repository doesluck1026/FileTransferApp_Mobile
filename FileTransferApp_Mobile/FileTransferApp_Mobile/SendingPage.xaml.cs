using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileTransferApp_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class SendingPage : ContentPage
    {
        public static SendingPage Instance;
        public string[] AvailableDeviceArray;
        public SendingPage()
        {
            InitializeComponent();
            Instance = this;
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (MainPage.Instance.AvailableDeviceList != null)
            {
                AvailableDeviceArray = MainPage.Instance.AvailableDeviceList.ToArray();
                list_Devices.ItemsSource = AvailableDeviceArray;
            }
        }
        private async void btn_SendFile_Clicked(object sender, EventArgs e)
        {
            bool didDeviceAccept = Main.ConnectToTargetDevice(txt_ClientIP.Text);
            if (didDeviceAccept)
            {
                Main.BeginSendingFiles();
                /// open the third page here
                await Navigation.PushModalAsync(new TransferPage());
            }

        }

        private void list_Devices_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            txt_ClientIP.Text = list_Devices.SelectedItem.ToString();
        }
    }
}