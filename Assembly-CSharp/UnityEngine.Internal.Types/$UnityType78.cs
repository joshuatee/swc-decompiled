using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType78 : $UnityType
	{
		public unsafe $UnityType78()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 361840) = ldftn($Invoke0);
			*(data + 361868) = ldftn($Invoke1);
			*(data + 361896) = ldftn($Invoke2);
			*(data + 361924) = ldftn($Invoke3);
			*(data + 361952) = ldftn($Invoke4);
			*(data + 361980) = ldftn($Invoke5);
			*(data + 362008) = ldftn($Invoke6);
			*(data + 362036) = ldftn($Invoke7);
			*(data + 362064) = ldftn($Invoke8);
			*(data + 1524056) = ldftn($Get0);
			*(data + 1524060) = ldftn($Set0);
			*(data + 1524072) = ldftn($Get1);
			*(data + 1524076) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new SpringPanel((UIntPtr)0);
		}
	}
}
