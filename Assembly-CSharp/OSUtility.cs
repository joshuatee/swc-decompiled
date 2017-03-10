using System;
using WinRTBridge;

public class OSUtility
{
	public delegate string GetVersionStringCallBack();

	public static OSUtility.GetVersionStringCallBack GetOSVersionString;

	public static string GetVersionString()
	{
		if (OSUtility.GetOSVersionString != null)
		{
			return OSUtility.GetOSVersionString();
		}
		return "1.0.0";
	}

	public OSUtility()
	{
	}

	protected internal OSUtility(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(OSUtility.GetVersionString());
	}
}
