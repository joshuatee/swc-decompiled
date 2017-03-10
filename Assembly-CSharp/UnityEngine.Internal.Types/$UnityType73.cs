using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType73 : $UnityType
	{
		public unsafe $UnityType73()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 360524) = ldftn($Invoke0);
			*(data + 360552) = ldftn($Invoke1);
			*(data + 360580) = ldftn($Invoke2);
			*(data + 360608) = ldftn($Invoke3);
			*(data + 360636) = ldftn($Invoke4);
			*(data + 360664) = ldftn($Invoke5);
			*(data + 360692) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new RealTime((UIntPtr)0);
		}
	}
}
