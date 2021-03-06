﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using FileTransferApp_Mobile.Resources;

namespace FileTransferApp_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferPage : ContentPage
    {
        private Timer UpdateTimer;
        private int updatePeriod = 50;          ///ms
        public TransferPage()
        {
            InitializeComponent();
            DeviceDisplay.KeepScreenOn = true;
            if (!Admob.TestMode)
                BannerView.AdsId = Admob.BannerAdID;
        }
        protected override void OnAppearing()
        {
            Admob.AdjustBannerView(BannerView);
            if (Main.IsSending)
                Main.BeginSendingFiles();
            else
                Main.ResponseToTransferRequest(true);
            Main.OnTransferFinished += Main_OnTransferFinished;
            Main.OnTransferAborted += Main_OnTransferAborted;
            Task.Run(() =>
            {
                while (!Main.IsTransfering)
                {
                    Thread.Sleep(1);
                }
                StartUpdatingUI();
            });            
        }     

        protected override void OnDisappearing()
        {
            Main.OnTransferFinished -= Main_OnTransferFinished;
            Main.OnTransferAborted -= Main_OnTransferAborted;
        }
        private  void Main_OnTransferFinished()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new TransferDonePage());
            });
        }
        private void Main_OnTransferAborted()
        {
            UserDialogs.Instance.Alert(AppResources.Transfer_AbortedMessage);
            Device.BeginInvokeOnMainThread(() =>
            {
                if(Main.FileNames.Length>0)
                    Navigation.PushAsync(new TransferDonePage());
                else
                    Navigation.PushAsync(new MainPage());
            });
        }
        protected override bool OnBackButtonPressed()
        {
            if(Main.IsTransfering)
            {
                UserDialogs.Instance.Alert(AppResources.Send_Warning_Goback);
            }
            else
            {
                Navigation.PushAsync(new Pages.ActionPage());
            }
            return true;
        }
        private void StartUpdatingUI()
        {
            UpdateTimer = new Timer(Timer_Tick, null, 0, updatePeriod);
            UpdateTimer.Change(0, updatePeriod);
        }
        private void StopUpdateingUI()
        {
            if (UpdateTimer != null)
            {
                UpdateTimer.Change(Timeout.Infinite, Timeout.Infinite);
                UpdateTimer.Dispose();
                UpdateTimer = null;
            }
        }
        private void Timer_Tick(object sender)
        {
            if (UpdateTimer == null)
                return;
            UpdateTimer.Change(Timeout.Infinite, Timeout.Infinite);
            Stopwatch stp = Stopwatch.StartNew();
            UpdateUI();
            long elapsedTime = stp.ElapsedMilliseconds;
            if(UpdateTimer!=null)
                UpdateTimer.Change(Math.Max(0, updatePeriod - elapsedTime), updatePeriod);
        }
        
        private void UpdateUI()
        {
            Dispatcher.BeginInvokeOnMainThread(() => {
                lbl_currentFileNumber.Text = Main.TransferMetrics.IndexOfCurrentFile.ToString();
                lbl_FileCount.Text = Main.TransferMetrics.CountOfFiles.ToString();
                lbl_FileSize.Text = Main.TransferMetrics.CurrentFile.FileSize.ToString("0.00") + " " + Main.TransferMetrics.CurrentFile.SizeUnit;
                lbl_FileName.Text = Main.TransferMetrics.CurrentFile.FileName;
                //lbl_progress.Text = "%"+Main.TransferMetrics.Progress.ToString("0.0");
                prg_Transfer.Percentage = (float)(Main.TransferMetrics.Progress / 100.0);
                lbl_transferSpeed.Text = Main.TransferMetrics.TransferSpeed.ToString("0.00") + " MB/s";
                lbl_PassedTime.Text = ((int)Main.TransferMetrics.TotalElapsedTime / 3600).ToString("00") + ":" + (((int)Main.TransferMetrics.TotalElapsedTime % 3600) / 60).ToString("00") + ":" +
                    (((int)Main.TransferMetrics.TotalElapsedTime % 3600) % 60).ToString("00");
                lbl_RemainingTime.Text = ((int)Main.TransferMetrics.EstimatedTime / 3600).ToString("00") + ":" + (((int)Main.TransferMetrics.EstimatedTime % 3600) / 60).ToString("00") + ":" +
                    (((int)Main.TransferMetrics.EstimatedTime % 3600) % 60).ToString("00");
                lbl_totalSent.Text = Main.TransferMetrics.TotalDataSent.ToString("0.00") + " "+Main.TransferMetrics.SentSizeUnit.ToString();
                lbl_totalSize.Text = Main.TransferMetrics.TotalDataSize.ToString("0.00") + " "+Main.TransferMetrics.SizeUnit.ToString();
            });
            
            if (!Main.IsTransfering)
                StopUpdateingUI();
        }
        private async void btn_AbortTransfer_Clicked(object sender, EventArgs e)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(AppResources.Transfer_ConfirmAbortMessage,AppResources.Transfer_ConfirmAbortTitle,
                AppResources.Transfer_ConfirmAbortYes, AppResources.Transfer_ConfirmAbortNo);
            if(result.ToString()== "True")
            {
                Main.OnTransferFinished -= Main_OnTransferFinished;
                Main.AbortTransfer();
                Device.BeginInvokeOnMainThread(() => 
                {
                    Navigation.PushAsync(new Pages.ActionPage());
                });
            }
        }
    }
}