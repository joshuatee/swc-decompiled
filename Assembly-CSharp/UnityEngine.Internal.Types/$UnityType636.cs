using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType636 : $UnityType
	{
		public unsafe $UnityType636()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 511108) = ldftn($Invoke0);
			*(data + 511136) = ldftn($Invoke1);
			*(data + 511164) = ldftn($Invoke2);
			*(data + 511192) = ldftn($Invoke3);
			*(data + 511220) = ldftn($Invoke4);
			*(data + 511248) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new PlanetEffectController((UIntPtr)0);
		}
	}
}
