using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType138 : $UnityType
	{
		public unsafe $UnityType138()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 381524) = ldftn($Invoke0);
			*(data + 381552) = ldftn($Invoke1);
			*(data + 381580) = ldftn($Invoke2);
			*(data + 381608) = ldftn($Invoke3);
			*(data + 381636) = ldftn($Invoke4);
			*(data + 381664) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new UICenterOnClick((UIntPtr)0);
		}
	}
}
