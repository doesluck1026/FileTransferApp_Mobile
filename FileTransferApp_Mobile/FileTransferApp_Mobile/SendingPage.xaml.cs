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
            if (MainPage.Instance.AvailableDeviceList!=null)
            {
                AvailableDeviceArray = MainPage.Instance.AvailableDeviceList.ToArray();
                list_Devices.ItemsSource = AvailableDeviceArray;
            }
        }

        private void btn_SendFile_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Buton basıldı");
            list_Devices.ItemsSource = new string[]
                {
                    "selam",
                    "sana",
                    "demir",
                    "ayak",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                    "demir",
                };
        }
    }
}