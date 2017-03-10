using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType100 : $UnityType
	{
		public unsafe $UnityType100()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 368952) = ldftn($Invoke0);
			*(data + 368980) = ldftn($Invoke1);
			*(data + 369008) = ldftn($Invoke2);
			*(data + 369036) = ldftn($Invoke3);
			*(data + 369064) = ldftn($Invoke4);
			*(data + 369092) = ldftn($Invoke5);
			*(data + 369120) = ldftn($Invoke6);
			*(data + 369148) = ldftn($Invoke7);
			*(data + 369176) = ldftn($Invoke8);
			*(data + 369204) = ldftn($Invoke9);
			*(data + 369232) = ldftn($Invoke10);
			*(data + 369260) = ldftn($Invoke11);
			*(data + 369288) = ldftn($Invoke12);
			*(data + 369316) = ldftn($Invoke13);
			*(data + 369344) = ldftn($Invoke14);
			*(data + 369372) = ldftn($Invoke15);
			*(data + 1524216) = ldftn($Get0);
			*(data + 1524220) = ldftn($Set0);
			*(data + 1524232) = ldftn($Get1);
			*(data + 1524236) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new TweenColor((UIntPtr)0);
		}
	}
}
