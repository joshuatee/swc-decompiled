using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType13 : $UnityType
	{
		public unsafe $UnityType13()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 348316) = ldftn($Invoke0);
			*(data + 348344) = ldftn($Invoke1);
			*(data + 348372) = ldftn($Invoke2);
			*(data + 348400) = ldftn($Invoke3);
			*(data + 348428) = ldftn($Invoke4);
			*(data + 348456) = ldftn($Invoke5);
			*(data + 348484) = ldftn($Invoke6);
			*(data + 1523896) = ldftn($Get0);
			*(data + 1523900) = ldftn($Set0);
		}

		public override object CreateInstance()
		{
			return new AnimatedAlpha((UIntPtr)0);
		}
	}
}
