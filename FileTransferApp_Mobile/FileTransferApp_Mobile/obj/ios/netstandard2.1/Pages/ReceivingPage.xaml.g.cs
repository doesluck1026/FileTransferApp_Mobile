//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("FileTransferApp_Mobile.Pages.ReceivingPage.xaml", "Pages/ReceivingPage.xaml", typeof(global::FileTransferApp_Mobile.Pages.ReceivingPage))]

namespace FileTransferApp_Mobile.Pages {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Pages\\ReceivingPage.xaml")]
    public partial class ReceivingPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label lbl_HostName;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label lbl_IP;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::FileTransferApp_Mobile.Controls.CustomLoader loader;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::MarcTron.Plugin.Controls.MTAdView BannerView;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(ReceivingPage));
            lbl_HostName = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "lbl_HostName");
            lbl_IP = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "lbl_IP");
            loader = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::FileTransferApp_Mobile.Controls.CustomLoader>(this, "loader");
            BannerView = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::MarcTron.Plugin.Controls.MTAdView>(this, "BannerView");
        }
    }
}