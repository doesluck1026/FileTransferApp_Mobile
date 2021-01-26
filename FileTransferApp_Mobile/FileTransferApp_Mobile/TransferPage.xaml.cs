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
    public partial class TransferPage : ContentPage
    {
        public TransferPage()
        {
            InitializeComponent();
            lbl_currentFileNumber.BindingContext = Main.TransferMetrics.IndexOfCurrentFile;
            lbl_FileCount.BindingContext = Main.TransferMetrics.CountOfFiles;
            lbl_FileSize.BindingContext = Main.TransferMetrics.CurrentFile.FileSize + " " + Main.TransferMetrics.CurrentFile.SizeUnit;
            lbl_FileName.BindingContext = Main.TransferMetrics.CurrentFile.FileName;
            lbl_progress.BindingContext = Main.TransferMetrics.Progress;
            prg_Transfer.BindingContext = Main.TransferMetrics.Progress / 100.0;
            lbl_PassedTime.BindingContext = ((int)Main.TransferMetrics.TotalElapsedTime / 3600).ToString("00") + ":" + (((int)Main.TransferMetrics.TotalElapsedTime % 3600) / 60).ToString("00") + 
                (((int)Main.TransferMetrics.TotalElapsedTime % 3600) % 60).ToString("00");
            lbl_RemainingTime.BindingContext= ((int)Main.TransferMetrics.EstimatedTime / 3600).ToString("00") + ":" + (((int)Main.TransferMetrics.EstimatedTime % 3600) / 60).ToString("00") +
                (((int)Main.TransferMetrics.EstimatedTime % 3600) % 60).ToString("00");

        }

        private void btn_AbortTransfer_Clicked(object sender, EventArgs e)
        {
            Main.AbortTransfer();
        }
    }
}