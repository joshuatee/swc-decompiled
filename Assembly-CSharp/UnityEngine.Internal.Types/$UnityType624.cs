using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType624 : $UnityType
	{
		public unsafe $UnityType624()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 508756) = ldftn($Invoke0);
			*(data + 508784) = ldftn($Invoke1);
			*(data + 508812) = ldftn($Invoke2);
			*(data + 508840) = ldftn($Invoke3);
			*(data + 508868) = ldftn($Invoke4);
			*(data + 508896) = ldftn($Invoke5);
			*(data + 508924) = ldftn($Invoke6);
			*(data + 508952) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new DefaultLightingEffects((UIntPtr)0);
		}
	}
}
