using StaRTS.Utils.Animation;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2546 : $UnityType
	{
		public unsafe $UnityType2546()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 993324) = ldftn($Invoke0);
			*(data + 993352) = ldftn($Invoke1);
			*(data + 993380) = ldftn($Invoke2);
			*(data + 993408) = ldftn($Invoke3);
			*(data + 993436) = ldftn($Invoke4);
			*(data + 993464) = ldftn($Invoke5);
			*(data + 993492) = ldftn($Invoke6);
			*(data + 993520) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new AnimController((UIntPtr)0);
		}
	}
}
