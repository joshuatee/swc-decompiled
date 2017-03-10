using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType168 : $UnityType
	{
		public unsafe $UnityType168()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 391520) = ldftn($Invoke0);
			*(data + 391548) = ldftn($Invoke1);
			*(data + 391576) = ldftn($Invoke2);
			*(data + 391604) = ldftn($Invoke3);
			*(data + 391632) = ldftn($Invoke4);
			*(data + 391660) = ldftn($Invoke5);
			*(data + 391688) = ldftn($Invoke6);
			*(data + 391716) = ldftn($Invoke7);
			*(data + 391744) = ldftn($Invoke8);
			*(data + 391772) = ldftn($Invoke9);
			*(data + 391800) = ldftn($Invoke10);
			*(data + 391828) = ldftn($Invoke11);
			*(data + 391856) = ldftn($Invoke12);
			*(data + 391884) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new UIKeyBinding((UIntPtr)0);
		}
	}
}
