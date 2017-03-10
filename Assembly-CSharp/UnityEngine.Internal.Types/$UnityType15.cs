using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType15 : $UnityType
	{
		public unsafe $UnityType15()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 348708) = ldftn($Invoke0);
			*(data + 348736) = ldftn($Invoke1);
			*(data + 348764) = ldftn($Invoke2);
			*(data + 348792) = ldftn($Invoke3);
			*(data + 348820) = ldftn($Invoke4);
			*(data + 348848) = ldftn($Invoke5);
			*(data + 348876) = ldftn($Invoke6);
			*(data + 1523928) = ldftn($Get0);
			*(data + 1523932) = ldftn($Set0);
			*(data + 1523944) = ldftn($Get1);
			*(data + 1523948) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new AnimatedWidget((UIntPtr)0);
		}
	}
}
