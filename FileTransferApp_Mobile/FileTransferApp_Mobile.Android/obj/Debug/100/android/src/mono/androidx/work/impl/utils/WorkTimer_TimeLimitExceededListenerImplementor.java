package mono.androidx.work.impl.utils;


public class WorkTimer_TimeLimitExceededListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		androidx.work.impl.utils.WorkTimer.TimeLimitExceededListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTimeLimitExceeded:(Ljava/lang/String;)V:GetOnTimeLimitExceeded_Ljava_lang_String_Handler:AndroidX.Work.Impl.Utils.WorkTimer/ITimeLimitExceededListenerInvoker, Xamarin.AndroidX.Work.Runtime\n" +
			"";
		mono.android.Runtime.register ("AndroidX.Work.Impl.Utils.WorkTimer+ITimeLimitExceededListenerImplementor, Xamarin.AndroidX.Work.Runtime", WorkTimer_TimeLimitExceededListenerImplementor.class, __md_methods);
	}


	public WorkTimer_TimeLimitExceededListenerImplementor ()
	{
		super ();
		if (getClass () == WorkTimer_TimeLimitExceededListenerImplementor.class)
			mono.android.TypeManager.Activate ("AndroidX.Work.Impl.Utils.WorkTimer+ITimeLimitExceededListenerImplementor, Xamarin.AndroidX.Work.Runtime", "", this, new java.lang.Object[] {  });
	}


	public void onTimeLimitExceeded (java.lang.String p0)
	{
		n_onTimeLimitExceeded (p0);
	}

	private native void n_onTimeLimitExceeded (java.lang.String p0);

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
