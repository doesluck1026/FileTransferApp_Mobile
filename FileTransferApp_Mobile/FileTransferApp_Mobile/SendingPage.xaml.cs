using System;
using System.Collections.Generic;
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
        public List<string[]> AvailableDeviceList = new List<string[]>();
        public SendingPage()
        {
            InitializeComponent();
            Instance = this;
        }

        private void btn_SendFile_Clicked(object sender, EventArgs e)
        {
            list_Devices = new ListView();

            list_Devices.ItemsSource = new string[]
                {
                    "selam",
                    "sana",
                    "demir",
                    "ayak"
                };
        }
        public void UpdateSource()
        {
            list_Devices = new ListView();
            list_Devices.ItemsSource = AvailableDeviceList;
        }
    }
}