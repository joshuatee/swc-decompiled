using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType110 : $UnityType
	{
		public unsafe $UnityType110()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 373152) = ldftn($Invoke0);
			*(data + 373180) = ldftn($Invoke1);
			*(data + 373208) = ldftn($Invoke2);
			*(data + 373236) = ldftn($Invoke3);
			*(data + 373264) = ldftn($Invoke4);
			*(data + 373292) = ldftn($Invoke5);
			*(data + 373320) = ldftn($Invoke6);
			*(data + 373348) = ldftn($Invoke7);
			*(data + 373376) = ldftn($Invoke8);
			*(data + 373404) = ldftn($Invoke9);
			*(data + 373432) = ldftn($Invoke10);
			*(data + 1524568) = ldftn($Get0);
			*(data + 1524572) = ldftn($Set0);
			*(data + 1524584) = ldftn($Get1);
			*(data + 1524588) = ldftn($Set1);
			*(data + 1524600) = ldftn($Get2);
			*(data + 1524604) = ldftn($Set2);
			*(data + 1524616) = ldftn($Get3);
			*(data + 1524620) = ldftn($Set3);
			*(data + 1524632) = ldftn($Get4);
			*(data + 1524636) = ldftn($Set4);
		}

		public override object CreateInstance()
		{
			return new TypewriterEffect((UIntPtr)0);
		}
	}
}
