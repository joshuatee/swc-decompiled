using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType178 : $UnityType
	{
		public unsafe $UnityType178()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 395944) = ldftn($Invoke0);
			*(data + 395972) = ldftn($Invoke1);
			*(data + 396000) = ldftn($Invoke2);
			*(data + 396028) = ldftn($Invoke3);
			*(data + 396056) = ldftn($Invoke4);
			*(data + 396084) = ldftn($Invoke5);
			*(data + 396112) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new UIOrthoCamera((UIntPtr)0);
		}
	}
}
