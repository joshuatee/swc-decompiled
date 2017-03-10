using StaRTS.Externals.IAP;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType422 : $UnityType
	{
		public unsafe $UnityType422()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 446596) = ldftn($Invoke0);
			*(data + 446624) = ldftn($Invoke1);
			*(data + 446652) = ldftn($Invoke2);
			*(data + 446680) = ldftn($Invoke3);
			*(data + 446708) = ldftn($Invoke4);
			*(data + 446736) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new WindowsIAPManager((UIntPtr)0);
		}
	}
}
