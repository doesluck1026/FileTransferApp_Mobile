package mono.com.google.android.gms.ads.initialization;


public class OnInitializationCompleteListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.ads.initialization.OnInitializationCompleteListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onInitializationComplete:(Lcom/google/android/gms/ads/initialization/InitializationStatus;)V:GetOnInitializationComplete_Lcom_google_android_gms_ads_initialization_InitializationStatus_Handler:Android.Gms.Ads.Initialization.IOnInitializationCompleteListenerInvoker, Xamarin.GooglePlayServices.Ads.Lite\n" +
			"";
		mono.android.Runtime.register ("Android.Gms.Ads.Initialization.IOnInitializationCompleteListenerImplementor, Xamarin.GooglePlayServices.Ads.Lite", OnInitializationCompleteListenerImplementor.class, __md_methods);
	}


	public OnInitializationCompleteListenerImplementor ()
	{
		super ();
		if (getClass () == OnInitializationCompleteListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Gms.Ads.Initialization.IOnInitializationCompleteListenerImplementor, Xamarin.GooglePlayServices.Ads.Lite", "", this, new java.lang.Object[] {  });
	}


	public void onInitializationComplete (com.google.android.gms.ads.initialization.InitializationStatus p0)
	{
		n_onInitializationComplete (p0);
	}

	private native void n_onInitializationComplete (com.google.android.gms.ads.initialization.InitializationStatus p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
