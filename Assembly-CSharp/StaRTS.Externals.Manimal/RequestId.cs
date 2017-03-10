using System;

namespace StaRTS.Externals.Manimal
{
	public static class RequestId
	{
		public static uint Id
		{
			get;
			private set;
		}

		public static void Reset()
		{
			RequestId.Id = 1u;
		}

		public static uint Get()
		{
			return RequestId.Id += 1u;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			RequestId.Reset();
			return -1L;
		}
	}
}
