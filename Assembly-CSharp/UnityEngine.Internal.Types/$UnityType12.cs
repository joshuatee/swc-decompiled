using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType12 : $UnityType
	{
		public unsafe $UnityType12()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 347868) = ldftn($Invoke0);
			*(data + 347896) = ldftn($Invoke1);
			*(data + 347924) = ldftn($Invoke2);
			*(data + 347952) = ldftn($Invoke3);
			*(data + 347980) = ldftn($Invoke4);
			*(data + 348008) = ldftn($Invoke5);
			*(data + 348036) = ldftn($Invoke6);
			*(data + 348064) = ldftn($Invoke7);
			*(data + 348092) = ldftn($Invoke8);
			*(data + 348120) = ldftn($Invoke9);
			*(data + 348148) = ldftn($Invoke10);
			*(data + 348176) = ldftn($Invoke11);
			*(data + 348204) = ldftn($Invoke12);
			*(data + 348232) = ldftn($Invoke13);
			*(data + 348260) = ldftn($Invoke14);
			*(data + 348288) = ldftn($Invoke15);
			*(data + 1523880) = ldftn($Get0);
			*(data + 1523884) = ldftn($Set0);
		}

		public override object CreateInstance()
		{
			return new ActiveAnimation((UIntPtr)0);
		}
	}
}
