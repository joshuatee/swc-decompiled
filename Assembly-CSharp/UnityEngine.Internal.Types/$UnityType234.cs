using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType234 : $UnityType
	{
		public unsafe $UnityType234()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 415488) = ldftn($Invoke0);
			*(data + 415516) = ldftn($Invoke1);
			*(data + 415544) = ldftn($Invoke2);
			*(data + 415572) = ldftn($Invoke3);
			*(data + 415600) = ldftn($Invoke4);
			*(data + 415628) = ldftn($Invoke5);
			*(data + 415656) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new UnityIAPManager((UIntPtr)0);
		}
	}
}
