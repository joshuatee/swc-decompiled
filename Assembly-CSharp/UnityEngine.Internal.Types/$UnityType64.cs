using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType64 : $UnityType
	{
		public unsafe $UnityType64()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 359404) = ldftn($Invoke0);
			*(data + 359432) = ldftn($Invoke1);
			*(data + 359460) = ldftn($Invoke2);
			*(data + 359488) = ldftn($Invoke3);
			*(data + 359516) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new PlanetaryLightingMonoBehaviour((UIntPtr)0);
		}
	}
}
