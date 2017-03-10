using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType37 : $UnityType
	{
		public unsafe $UnityType37()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 352712) = ldftn($Invoke0);
			*(data + 352740) = ldftn($Invoke1);
			*(data + 352768) = ldftn($Invoke2);
			*(data + 352796) = ldftn($Invoke3);
			*(data + 352824) = ldftn($Invoke4);
			*(data + 352852) = ldftn($Invoke5);
			*(data + 352880) = ldftn($Invoke6);
			*(data + 352908) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new EventDelegate.Parameter((UIntPtr)0);
		}
	}
}
