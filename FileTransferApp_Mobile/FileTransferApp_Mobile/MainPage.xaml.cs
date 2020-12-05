using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FileTransferApp_Mobile
{
    public partial class MainPage : ContentPage
    {        
        public MainPage()
        {
            InitializeComponent();
            Img_FirstStep.Source = (Device.RuntimePlatform == Device.Android) ? ImageSource.FromFile("icons/number1.png") : ImageSource.FromFile("icons/number1.png");
            Img_SecondStep.Source = (Device.RuntimePlatform == Device.Android) ? ImageSource.FromFile("icons/number2.png") : ImageSource.FromFile("icons/number2.png");
            Img_ThirdStep.Source = (Device.RuntimePlatform == Device.Android) ? ImageSource.FromFile("icons/number3.png") : ImageSource.FromFile("icons/number3.png");
        }
        private void btn_SendFile_Click(object sender, EventArgs e)
        {
            txt_FilePath.Text = "Do you really wanna send?";
            //StopFlashing();
            //string FileURL = SelectFile();
            //if (FileURL == null)
            //{
            //    string sSelectionValidFile = res_man.GetString("sSelectionValidFile", cul);
            //    MessageBox.Show(sSelectionValidFile);
            //    return;
            //}
            //Reset();
            //TransferMode = FileOperations.TransferMode.Send;
            //Main.SetFileURL(FileURL);
            //FlashObject(txt_IpCode);
            //System.Diagnostics.Debug.WriteLine(" FileURL = " + FileURL);
        }
        private void btn_ReceiveFile_Click(object sender, EventArgs e)
        {
            txt_FilePath.Text = "Do you really wanna receive?";

            //StopFlashing();
            //FileURL = GetFolder();
            //if (FileURL == null)
            //{
            //    System.Diagnostics.Debug.WriteLine("File Url is null");
            //    return;
            //}
            //Reset();
            //TransferMode = FileOperations.TransferMode.Receive;
            //Main.FirstStep = true;
            //Main.InfoMsg = "sEnterCode";
            //FlashObject(txt_IpCode);
            //System.Diagnostics.Debug.WriteLine(" FileURL = " + FileURL);
        }
        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            //if (TransferMode == FileOperations.TransferMode.Receive)
            //{
            //    Main.Init(false);
            //    string code = txt_IpCode.Text;
            //    bool success = Main.EnterTheCode(code);
            //    if (success)
            //    {
            //        StopFlashing();
            //        Main.SetFilePathToSave(FileURL);
            //        string FileName = Main.FileName;
            //        string fileSizeType = Main.FileSizeType.ToString();
            //        string fileSize = Main.FileSize.ToString("0.00") + " " + fileSizeType;

            //        string sImportingVerification = res_man.GetString("sImportingVerification", cul);
            //        string sConfirmation = res_man.GetString("sConfirmation", cul);
            //        MessageBoxResult result = MessageBox.Show(sImportingVerification + "\n" + FileName + " file of " + fileSize + " size?", sConfirmation, MessageBoxButton.YesNo);
            //        if (result == MessageBoxResult.Yes)
            //        {
            //            // Yes code here  
            //            Main.RespondToTransferRequest(true);
            //        }
            //        else if (result == MessageBoxResult.No)
            //        {
            //            // No code here  
            //            Main.RespondToTransferRequest(false);
            //        }

            //        // Gönderimin alınıp alınmaması durmu burada sorulur
            //    }
            //    else
            //    {
            //        MessageBox.Show("Entered code is incorrect!");
            //        // kodun hatalı olduğu ise burada gösterilri
            //    }
            //}
            //else if (TransferMode == FileOperations.TransferMode.Send && Communication.isClientConnected)
            //{
            //    Main.TransferApproved = true;
            //    StopFlashing();
            //}
            pbStatus.Progress += 0.1;
            if (pbStatus.Progress >= 1)
                pbStatus.Progress = 0;
        }
        /// <summary>
        /// This function is used to prevent the user to type more than 6 characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_IpCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = txt_IpCode.Text;
            if (text.Length > 6)
            {
                txt_IpCode.Text = text.Remove(6, 1);
               // txt_IpCode.CaretIndex = 6;
            }
        }
    }
}
