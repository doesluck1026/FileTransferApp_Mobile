package mono.com.google.android.gms.ads.mediation.customevent;


public class CustomEventListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.ads.mediation.customevent.CustomEventListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAdClicked:()V:GetOnAdClickedHandler:Android.Gms.Ads.Mediation.CustomEvent.ICustomEventListenerInvoker, Xamarin.GooglePlayServices.Ads.Lite\n" +
			"n_onAdClosed:()V:GetOnAdClosedHandler:Android.Gms.Ads.Mediation.CustomEvent.ICustomEventListenerInvoker, Xamarin.GooglePlayServices.Ads.Lite\n" +
			"n_onAdFailedToLoad:(Lcom/google/android/gms/ads/AdError;)V:GetOnAdFailedToLoad2_Lcom_google_android_gms_ads_AdError_Handler:Android.Gms.Ads.Mediation.CustomEvent.ICustomEventListenerInvoker, Xamarin.GooglePlayServices.Ads.Lite\n" +
			"n_onAdFailedToLoad:(I)V:GetOnAdFailedToLoad_IHandler:Android.Gms.Ads.Mediation.CustomEvent.ICustomEventListenerInvoker, Xamarin.GooglePlayServices.Ads.Lite\n" +
			"n_onAdLeftApplication:()V:GetOnAdLeftApplicationHandler:Android.Gms.Ads.Mediation.CustomEvent.ICustomEventListenerInvoker, Xamarin.GooglePlayServices.Ads.Lite\n" +
			"n_onAdOpened:()V:GetOnAdOpenedHandler:Android.Gms.Ads.Mediation.CustomEvent.ICustomEventListenerInvoker, Xamarin.GooglePlayServices.Ads.Lite\n" +
			"";
		mono.android.Runtime.register ("Android.Gms.Ads.Mediation.CustomEvent.ICustomEventListenerImplementor, Xamarin.GooglePlayServices.Ads.Lite", CustomEventListenerImplementor.class, __md_methods);
	}


	public CustomEventListenerImplementor ()
	{
		super ();
		if (getClass () == CustomEventListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Gms.Ads.Mediation.CustomEvent.ICustomEventListenerImplementor, Xamarin.GooglePlayServices.Ads.Lite", "", this, new java.lang.Object[] {  });
	}


	public void onAdClicked ()
	{
		n_onAdClicked ();
	}

	private native void n_onAdClicked ();


	public void onAdClosed ()
	{
		n_onAdClosed ();
	}

	private native void n_onAdClosed ();


	public void onAdFailedToLoad (com.google.android.gms.ads.AdError p0)
	{
		n_onAdFailedToLoad (p0);
	}

	private native void n_onAdFailedToLoad (com.google.android.gms.ads.AdError p0);


	public void onAdFailedToLoad (int p0)
	{
		n_onAdFailedToLoad (p0);
	}

	private native void n_onAdFailedToLoad (int p0);


	public void onAdLeftApplication ()
	{
		n_onAdLeftApplication ();
	}

	private native void n_onAdLeftApplication ();


	public void onAdOpened ()
	{
		n_onAdOpened ();
	}

	private native void n_onAdOpened ();

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
