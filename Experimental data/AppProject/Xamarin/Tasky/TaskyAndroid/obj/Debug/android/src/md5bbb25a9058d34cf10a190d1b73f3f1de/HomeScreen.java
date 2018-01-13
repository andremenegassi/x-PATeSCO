package md5bbb25a9058d34cf10a190d1b73f3f1de;


public class HomeScreen
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"";
		mono.android.Runtime.register ("TaskyAndroid.Screens.HomeScreen, TaskyAndroid, Version=1.0.6437.1051, Culture=neutral, PublicKeyToken=null", HomeScreen.class, __md_methods);
	}


	public HomeScreen () throws java.lang.Throwable
	{
		super ();
		if (getClass () == HomeScreen.class)
			mono.android.TypeManager.Activate ("TaskyAndroid.Screens.HomeScreen, TaskyAndroid, Version=1.0.6437.1051, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();

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
