using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType931 : $UnityType
	{
		public unsafe $UnityType931()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 596620) = ldftn($Invoke0);
			*(data + 596648) = ldftn($Invoke1);
			*(data + 596676) = ldftn($Invoke2);
			*(data + 596704) = ldftn($Invoke3);
			*(data + 596732) = ldftn($Invoke4);
			*(data + 596760) = ldftn($Invoke5);
			*(data + 596788) = ldftn($Invoke6);
			*(data + 596816) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new PlayerContentStartupTask((UIntPtr)0);
		}
	}
}
