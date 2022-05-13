using Android.App;
using Android.Runtime;
using System;

namespace PlaygroundAndroid;

[Application]
public class MainApplication : Android.App.Application
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}
}
