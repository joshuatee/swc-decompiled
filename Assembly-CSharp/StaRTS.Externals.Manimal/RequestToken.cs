using System;
using WinRTBridge;

namespace StaRTS.Externals.Manimal
{
	public static class RequestToken
	{
		public static string Get()
		{
			return Guid.NewGuid().ToString();
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RequestToken.Get());
		}
	}
}
