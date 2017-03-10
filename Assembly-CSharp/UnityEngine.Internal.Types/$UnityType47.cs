using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType47 : $UnityType
	{
		public unsafe $UnityType47()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 353720) = ldftn($Invoke0);
			*(data + 353748) = ldftn($Invoke1);
			*(data + 353776) = ldftn($Invoke2);
			*(data + 353804) = ldftn($Invoke3);
			*(data + 353832) = ldftn($Invoke4);
			*(data + 353860) = ldftn($Invoke5);
			*(data + 353888) = ldftn($Invoke6);
			*(data + 353916) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new LanguageSelection((UIntPtr)0);
		}
	}
}
