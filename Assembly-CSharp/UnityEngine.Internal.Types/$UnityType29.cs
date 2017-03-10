using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType29 : $UnityType
	{
		public unsafe $UnityType29()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 351396) = ldftn($Invoke0);
			*(data + 351424) = ldftn($Invoke1);
			*(data + 351452) = ldftn($Invoke2);
			*(data + 351480) = ldftn($Invoke3);
			*(data + 351508) = ldftn($Invoke4);
			*(data + 351536) = ldftn($Invoke5);
			*(data + 351564) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new ByteReader((UIntPtr)0);
		}
	}
}
