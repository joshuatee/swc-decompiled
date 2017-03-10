using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType33 : $UnityType
	{
		public unsafe $UnityType33()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 351648) = ldftn($Invoke0);
			*(data + 351676) = ldftn($Invoke1);
			*(data + 351704) = ldftn($Invoke2);
			*(data + 351732) = ldftn($Invoke3);
			*(data + 351760) = ldftn($Invoke4);
			*(data + 351788) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DisableObjectForWindows((UIntPtr)0);
		}
	}
}
