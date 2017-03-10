using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType157 : $UnityType
	{
		public unsafe $UnityType157()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 388804) = ldftn($Invoke0);
			*(data + 388832) = ldftn($Invoke1);
			*(data + 388860) = ldftn($Invoke2);
			*(data + 388888) = ldftn($Invoke3);
			*(data + 388916) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new UIGeometry((UIntPtr)0);
		}
	}
}
