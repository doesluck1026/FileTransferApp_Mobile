using System;
using System.Collections.Generic;
using System.Text;
using MarcTron.Plugin;

namespace FileTransferApp_Mobile
{
    class Admob
    {
        #region Parameters and ID's

        private static string InterstitialAdID = "ca-app-pub-9174890030718712/5478349310";
        private static string Test_InterstitialAdID= "ca-app-pub-3940256099942544/8691691433";
        public static string BannerAdID = "ca-app-pub-9174890030718712/5131413691";
        public static readonly bool TestMode = true;
        #endregion

        #region Public Variables
        public static bool IsInterstitialLoaded
        {
            get
            {
                lock (Lck_IsInterstitialLoaded)
                    return _isInterstitialLoaded;
            }
            private set
            {
                lock (Lck_IsInterstitialLoaded)
                    _isInterstitialLoaded = value;
            }
        }

        #endregion

        #region Private Variables

        private static bool _isInterstitialLoaded = false;
        private static bool didInit = false;
        #endregion

        #region Private Objects

        private static object Lck_IsInterstitialLoaded = new object();

        #endregion

        public static void LoadInterstitialAd()
        {
            if(TestMode)
                CrossMTAdmob.Current.LoadInterstitial(Test_InterstitialAdID);
            else
                CrossMTAdmob.Current.LoadInterstitial(InterstitialAdID);
            if(!didInit)
            {
                CrossMTAdmob.Current.OnInterstitialLoaded += Current_OnInterstitialLoaded;
                didInit = true;
            }
        }
        public static void ShowInterstitialAd()
        {
            IsInterstitialLoaded = false;
            CrossMTAdmob.Current.ShowInterstitial();
            LoadInterstitialAd();
        }
        private static void Current_OnInterstitialLoaded(object sender, EventArgs e)
        {
            IsInterstitialLoaded = true;
        }
    }
}
