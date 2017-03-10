using StaRTS.Externals.DMOAnalytics;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType384 : $UnityType
	{
		public unsafe $UnityType384()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 436964) = ldftn($Invoke0);
			*(data + 436992) = ldftn($Invoke1);
			*(data + 437020) = ldftn($Invoke2);
			*(data + 437048) = ldftn($Invoke3);
			*(data + 437076) = ldftn($Invoke4);
			*(data + 437104) = ldftn($Invoke5);
			*(data + 437132) = ldftn($Invoke6);
			*(data + 437160) = ldftn($Invoke7);
			*(data + 437188) = ldftn($Invoke8);
			*(data + 437216) = ldftn($Invoke9);
			*(data + 437244) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new DefaultDMOAnalyticsManager((UIntPtr)0);
		}
	}
}
