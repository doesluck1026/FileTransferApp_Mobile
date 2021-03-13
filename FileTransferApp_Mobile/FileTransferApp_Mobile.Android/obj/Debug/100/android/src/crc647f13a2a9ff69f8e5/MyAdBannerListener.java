package crc647f13a2a9ff69f8e5;


public class MyAdBannerListener
	extends com.google.android.gms.ads.AdListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAdClicked:()V:GetOnAdClickedHandler\n" +
			"n_onAdClosed:()V:GetOnAdClosedHandler\n" +
			"n_onAdImpression:()V:GetOnAdImpressionHandler\n" +
			"n_onAdOpened:()V:GetOnAdOpenedHandler\n" +
			"n_onAdFailedToLoad:(I)V:GetOnAdFailedToLoad_IHandler\n" +
			"n_onAdLeftApplication:()V:GetOnAdLeftApplicationHandler\n" +
			"n_onAdLoaded:()V:GetOnAdLoadedHandler\n" +
			"";
		mono.android.Runtime.register ("MarcTron.Plugin.Listeners.MyAdBannerListener, Plugin.MtAdmob", MyAdBannerListener.class, __md_methods);
	}


	public MyAdBannerListener ()
	{
		super ();
		if (getClass () == MyAdBannerListener.class)
			mono.android.TypeManager.Activate ("MarcTron.Plugin.Listeners.MyAdBannerListener, Plugin.MtAdmob", "", this, new java.lang.Object[] {  });
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


	public void onAdImpression ()
	{
		n_onAdImpression ();
	}

	private native void n_onAdImpression ();


	public void onAdOpened ()
	{
		n_onAdOpened ();
	}

	private native void n_onAdOpened ();


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


	public void onAdLoaded ()
	{
		n_onAdLoaded ();
	}

	private native void n_onAdLoaded ();

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
