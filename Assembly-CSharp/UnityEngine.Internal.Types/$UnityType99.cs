using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType99 : $UnityType
	{
		public unsafe $UnityType99()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 368560) = ldftn($Invoke0);
			*(data + 368588) = ldftn($Invoke1);
			*(data + 368616) = ldftn($Invoke2);
			*(data + 368644) = ldftn($Invoke3);
			*(data + 368672) = ldftn($Invoke4);
			*(data + 368700) = ldftn($Invoke5);
			*(data + 368728) = ldftn($Invoke6);
			*(data + 368756) = ldftn($Invoke7);
			*(data + 368784) = ldftn($Invoke8);
			*(data + 368812) = ldftn($Invoke9);
			*(data + 368840) = ldftn($Invoke10);
			*(data + 368868) = ldftn($Invoke11);
			*(data + 368896) = ldftn($Invoke12);
			*(data + 368924) = ldftn($Invoke13);
			*(data + 1524184) = ldftn($Get0);
			*(data + 1524188) = ldftn($Set0);
			*(data + 1524200) = ldftn($Get1);
			*(data + 1524204) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new TweenAlpha((UIntPtr)0);
		}
	}
}
