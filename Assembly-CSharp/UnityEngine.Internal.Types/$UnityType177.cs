using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType177 : $UnityType
	{
		public unsafe $UnityType177()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 395692) = ldftn($Invoke0);
			*(data + 395720) = ldftn($Invoke1);
			*(data + 395748) = ldftn($Invoke2);
			*(data + 395776) = ldftn($Invoke3);
			*(data + 395804) = ldftn($Invoke4);
			*(data + 395832) = ldftn($Invoke5);
			*(data + 395860) = ldftn($Invoke6);
			*(data + 395888) = ldftn($Invoke7);
			*(data + 395916) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new UILocalize((UIntPtr)0);
		}
	}
}
