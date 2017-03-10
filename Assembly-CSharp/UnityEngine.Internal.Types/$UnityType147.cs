using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType147 : $UnityType
	{
		public unsafe $UnityType147()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 383820) = ldftn($Invoke0);
			*(data + 383848) = ldftn($Invoke1);
			*(data + 383876) = ldftn($Invoke2);
			*(data + 383904) = ldftn($Invoke3);
			*(data + 383932) = ldftn($Invoke4);
			*(data + 383960) = ldftn($Invoke5);
			*(data + 383988) = ldftn($Invoke6);
			*(data + 384016) = ldftn($Invoke7);
			*(data + 1526008) = ldftn($Get0);
			*(data + 1526012) = ldftn($Set0);
			*(data + 1526024) = ldftn($Get1);
			*(data + 1526028) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new UIDragResize((UIntPtr)0);
		}
	}
}
