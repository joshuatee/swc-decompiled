using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType646 : $UnityType
	{
		public unsafe $UnityType646()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 513404) = ldftn($Invoke0);
			*(data + 513432) = ldftn($Invoke1);
			*(data + 513460) = ldftn($Invoke2);
			*(data + 513488) = ldftn($Invoke3);
			*(data + 513516) = ldftn($Invoke4);
			*(data + 513544) = ldftn($Invoke5);
			*(data + 513572) = ldftn($Invoke6);
			*(data + 513600) = ldftn($Invoke7);
			*(data + 513628) = ldftn($Invoke8);
			*(data + 513656) = ldftn($Invoke9);
			*(data + 513684) = ldftn($Invoke10);
			*(data + 513712) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new TerrainBlendController((UIntPtr)0);
		}
	}
}
