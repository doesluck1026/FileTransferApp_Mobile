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
using Plugin.FilePicker;
using System.IO;


namespace FileTransferApp_Mobile
{
    public partial class MainPage : ContentPage
    {
        private string FileURL = "";
        private string SaveFolderPath= Environment.GetFolderPath(Environment.SpecialFolder.Personal)+"\\WifiFileTransfer";
        private FileOperations.TransferMode TransferMode;
        private uint prev_timePassed = 0;
        private double _transferSpeed = 0;
        private uint mb = 1024 * 1024;
        private uint ETA = 0;
        private Thread UIUpdate_thread;
        private bool UIUpdate_Start = false;
        private int UIUpdate_Period = 100;      // in ms
        private Brush CompletedStep = Brush.LimeGreen;
        private Brush CurrentStep = Brush.Orange;
        private Brush UnCompletedStep = Brush.LightBlue;
        ResourceManager res_man;    // declare Resource manager to access to specific cultureinfo
        CultureInfo cul;            //declare culture info
        ImageSource btm_checked = ImageSource.FromFile("icons/checked.png");
        public MainPage()
        {
            InitializeComponent();
            Img_FirstStep.Source = (Device.RuntimePlatform == Device.Android) ? ImageSource.FromFile("icons/number1.png") : ImageSource.FromFile("icons/number1.png");
            Img_SecondStep.Source = (Device.RuntimePlatform == Device.Android) ? ImageSource.FromFile("icons/number2.png") : ImageSource.FromFile("icons/number2.png");
            Img_ThirdStep.Source = (Device.RuntimePlatform == Device.Android) ? ImageSource.FromFile("icons/number3.png") : ImageSource.FromFile("icons/number3.png");

            
            Main.Init(true);
            UI_Init();

            UIUpdate_thread = new Thread(UpdateUI);
            UIUpdate_thread.IsBackground = true;
            UIUpdate_Start = true;
            UIUpdate_thread.Start();
        }
        private void btn_SendFile_Click(object sender, EventArgs e)
        {
            txt_FilePath.Text = "Do you really wanna send?";
            StopFlashing();
            
            SelectFile();
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
            if (TransferMode == FileOperations.TransferMode.Send && Communication.isClientConnected)
            {
                Main.TransferApproved = true;
                Main.ExportingVerification = false;
                StopFlashing();
            }
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
        private void UpdateUI()
        {
            Stopwatch UpdateWatch = new Stopwatch();

            while (UIUpdate_Start)
            {
                UpdateWatch.Restart();
                Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    if (Main.FirstStep)
                    {

                        Img_FirstStep.Source = btm_checked;

                        //border_SecondStep.IsEnabled = true;
                        btn_Confirm.IsEnabled = true;
                        Main.FirstStep = false;
                    }
                    else if (Main.SecondStep)
                    {

                        Img_SecondStep.Source = btm_checked;

                        //border_ThirdStep.IsEnabled = true;
                        if (TransferMode == FileOperations.TransferMode.Send) // ********************* Main içerisinde de proses tipi var biri seçilmeli
                        {
                           // btn_Confirm.IsEnabled = false;
                        }
                        Main.SecondStep = false;
                        StopFlashing();
                    }
                    else if (Main.ThirdStep)
                    {
                        Img_ThirdStep.Source = btm_checked;
                        Main.ThirdStep = false;


                    }

                    if (Main.ExportingVerification && Communication.isClientConnected)
                    {
                        //string sExportingVerification = res_man.GetString("sExportingVerification", cul) + ":" + Main.HostName.ToString();
                        //string sConfirmation = res_man.GetString("sConfirmation", cul);
                       // MessageBoxResult result = MessageBox.Show(sExportingVerification, sConfirmation, MessageBoxButton.YesNo);
                        //if (result == MessageBoxResult.Yes)
                         

                    }


                    if (TransferMode == FileOperations.TransferMode.Send)
                        txt_IpCode.Text = Main.IpCode;
                    string MainStatus = Main.InfoMsg;
                    //if (!MainStatus.Equals(""))
                   //     txt_StatusInfo.Text = res_man.GetString(MainStatus, cul);
                    txt_FilePath.Text = Main.URL;
                    txt_FileName.Text = Main.FileName;
                    txt_HostName.Text = Main.HostName;
                    if (Main.FileSizeType != Communication.SizeTypes.none)
                        txt_FileSize.Text = Main.FileSize.ToString("0.00") + " " + Main.FileSizeType.ToString();
                    txt_TransferSpeed.Text = Main.TransferSpeed.ToString("0.00") + " MB/s";
                    txt_EstimatedTime.Text = Main.EstimatedMin.ToString() + " : " + Main.EstimatedSec.ToString();
                    txt_PassedTime.Text = Main.PassedMin.ToString() + " : " + Main.PassedSec.ToString();
                    pbStatus.Progress = Main.CompletedPercentage;
                });

                while (UpdateWatch.ElapsedMilliseconds < UIUpdate_Period)
                {

                }
            }

        }
        private void Reset()
        {
            Main.TransferApproved = false;
            Main.FirstStep = true;
            Main.SecondStep = false;
            Main.ThirdStep = false;
            Main.CompletedPercentage = 0;
            Main.PassedMin = 0;
            Main.PassedSec = 0;
            Main.EstimatedMin = 0;
            Main.EstimatedSec = 0;

            Img_FirstStep.Source = (Device.RuntimePlatform == Device.Android) ? ImageSource.FromFile("icons/number1.png") : ImageSource.FromFile("icons/number1.png");
            Img_SecondStep.Source = (Device.RuntimePlatform == Device.Android) ? ImageSource.FromFile("icons/number2.png") : ImageSource.FromFile("icons/number2.png");
            Img_ThirdStep.Source = (Device.RuntimePlatform == Device.Android) ? ImageSource.FromFile("icons/number3.png") : ImageSource.FromFile("icons/number3.png");

            Main.Reset();
            txt_FilePath.Text = "";
            txt_FileName.Text = "";
            txt_FileSize.Text = "";
            txt_HostName.Text = "";
            txt_EstimatedTime.Text = "";
            txt_PassedTime.Text = "";
            txt_IpCode.Text = "";
            StopFlashing();
        }
        /// <summary>
        /// The address of the file to be processed is selected
        /// </summary>
        /// <returns>the address of the file in memory</returns>
        private async void SelectFile()
        {
            var file = await CrossFilePicker.Current.PickFile();

            if (file != null)
            {
                
                lbl_FileName.Text = file.FileName;
                Debug.WriteLine("FileName: " + file.FilePath+"/"+file.FileName + "   len: " + file.DataArray.Length);
                this.FileURL = file.FilePath;
                Reset();
                TransferMode = FileOperations.TransferMode.Send;
                Main.SetFileURL(FileURL);
                FlashObject(txt_IpCode);
            }
            //Main.SetFileURL(FileURL);
            //return null;
        }
       
        /// <summary>
        /// this function is used to select a folder on current machine and returns folder path
        /// </summary>
        /// <returns>Folder path</returns>
        private string GetFolder()
        {

            return null;
        }
        private void UI_Init()
        {
           // border_FirstStep.IsEnabled = true;
            //border_SecondStep.IsEnabled = false;
            //border_ThirdStep.IsEnabled = false;

            //lbl_FirstStep.Fill = CurrentStep;
            //lbl_SecondStep.Fill = UnCompletedStep;
            //lbl_ThirdStep.Fill = UnCompletedStep;



        }

        private void switch_language()
        {
            if (res_man != null)
            {
                btn_SendFile.Text = res_man.GetString("sSendFile", cul);
                btn_ReceiveFile.Text = res_man.GetString("sReceiveFile", cul);
                btn_Confirm.Text = res_man.GetString("sConfirmation", cul);

                lbl_FilePath.Text = res_man.GetString("sFilePath", cul);
                lbl_FileName.Text = res_man.GetString("sFileName", cul);
                lbl_FileSize.Text = res_man.GetString("sFileSize", cul);
                lbl_HostName.Text = res_man.GetString("sHostName", cul);

                lbl_TransferSpeed.Text = res_man.GetString("sSpeed", cul);
                lbl_PassedTime.Text = res_man.GetString("sTimePassed", cul);
                lbl_EstimatedTime.Text = res_man.GetString("sEstimatedTime", cul);
                lbl_code.Text = res_man.GetString("sCode", cul);
            }
        }
        Task FlashTask;
        private void FlashObject(Entry obj)
        {
            Color originalBrush = obj.BackgroundColor;
            isFlashing = true;
            FlashTask = Task.Run(() => Flash(obj, originalBrush));
            Debug.WriteLine("Flashing is started!");

        }
        private void StopFlashing()
        {
            isFlashing = false;
            Debug.WriteLine("Flashing is stopped!");
        }
        private bool isFlashing
        {
            get
            {
                lock (lck_isFlashing)
                    return _isFlashing;
            }
            set
            {
                lock (lck_isFlashing)
                    _isFlashing = value;
            }
        }
        private bool _isFlashing;
        private object lck_isFlashing = new object();
        private void Flash(Entry obj, Color origBrush)
        {
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                obj.BackgroundColor = Color.CornflowerBlue;
            });
            Thread.Sleep(500);
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                obj.BackgroundColor = origBrush;
            });
            Thread.Sleep(500);

            if (isFlashing)
                Flash(obj, origBrush);
        }
    }
}
