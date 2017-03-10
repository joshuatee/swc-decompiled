using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType67 : $UnityType
	{
		public unsafe $UnityType67()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 359544) = ldftn($Invoke0);
			*(data + 359572) = ldftn($Invoke1);
			*(data + 359600) = ldftn($Invoke2);
			*(data + 359628) = ldftn($Invoke3);
			*(data + 359656) = ldftn($Invoke4);
			*(data + 359684) = ldftn($Invoke5);
			*(data + 359712) = ldftn($Invoke6);
			*(data + 359740) = ldftn($Invoke7);
			*(data + 359768) = ldftn($Invoke8);
			*(data + 359796) = ldftn($Invoke9);
			*(data + 359824) = ldftn($Invoke10);
			*(data + 1524040) = ldftn($Get0);
			*(data + 1524044) = ldftn($Set0);
		}

		public override object CreateInstance()
		{
			return new PropertyBinding((UIntPtr)0);
		}
	}
}
