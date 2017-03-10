using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType209 : $UnityType
	{
		public unsafe $UnityType209()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 408236) = ldftn($Invoke0);
			*(data + 408264) = ldftn($Invoke1);
			*(data + 408292) = ldftn($Invoke2);
			*(data + 408320) = ldftn($Invoke3);
			*(data + 408348) = ldftn($Invoke4);
			*(data + 408376) = ldftn($Invoke5);
			*(data + 408404) = ldftn($Invoke6);
			*(data + 408432) = ldftn($Invoke7);
			*(data + 408460) = ldftn($Invoke8);
			*(data + 408488) = ldftn($Invoke9);
			*(data + 408516) = ldftn($Invoke10);
			*(data + 408544) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new UISpriteData((UIntPtr)0);
		}
	}
}
