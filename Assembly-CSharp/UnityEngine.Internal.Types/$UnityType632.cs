using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType632 : $UnityType
	{
		public unsafe $UnityType632()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 510016) = ldftn($Invoke0);
			*(data + 510044) = ldftn($Invoke1);
			*(data + 510072) = ldftn($Invoke2);
			*(data + 510100) = ldftn($Invoke3);
			*(data + 510128) = ldftn($Invoke4);
			*(data + 510156) = ldftn($Invoke5);
			*(data + 510184) = ldftn($Invoke6);
			*(data + 510212) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new LightSaberHitEffect((UIntPtr)0);
		}
	}
}
