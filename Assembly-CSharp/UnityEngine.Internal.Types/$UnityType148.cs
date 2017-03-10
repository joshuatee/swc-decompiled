using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType148 : $UnityType
	{
		public unsafe $UnityType148()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 384044) = ldftn($Invoke0);
			*(data + 384072) = ldftn($Invoke1);
			*(data + 384100) = ldftn($Invoke2);
			*(data + 384128) = ldftn($Invoke3);
			*(data + 384156) = ldftn($Invoke4);
			*(data + 384184) = ldftn($Invoke5);
			*(data + 384212) = ldftn($Invoke6);
			*(data + 384240) = ldftn($Invoke7);
			*(data + 384268) = ldftn($Invoke8);
			*(data + 384296) = ldftn($Invoke9);
			*(data + 384324) = ldftn($Invoke10);
			*(data + 384352) = ldftn($Invoke11);
			*(data + 1526040) = ldftn($Get0);
			*(data + 1526044) = ldftn($Set0);
			*(data + 1526056) = ldftn($Get1);
			*(data + 1526060) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new UIDragScrollView((UIntPtr)0);
		}
	}
}
