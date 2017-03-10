using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1741 : $UnityType
	{
		public unsafe $UnityType1741()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 675692) = ldftn($Invoke0);
			*(data + 675720) = ldftn($Invoke1);
			*(data + 675748) = ldftn($Invoke2);
			*(data + 675776) = ldftn($Invoke3);
			*(data + 675804) = ldftn($Invoke4);
			*(data + 675832) = ldftn($Invoke5);
			*(data + 675860) = ldftn($Invoke6);
			*(data + 675888) = ldftn($Invoke7);
			*(data + 675916) = ldftn($Invoke8);
			*(data + 675944) = ldftn($Invoke9);
			*(data + 675972) = ldftn($Invoke10);
			*(data + 676000) = ldftn($Invoke11);
			*(data + 676028) = ldftn($Invoke12);
			*(data + 676056) = ldftn($Invoke13);
			*(data + 676084) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new TroopComponent((UIntPtr)0);
		}
	}
}
