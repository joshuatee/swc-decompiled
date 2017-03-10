using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType691 : $UnityType
	{
		public unsafe $UnityType691()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 528776) = ldftn($Invoke0);
			*(data + 528804) = ldftn($Invoke1);
			*(data + 528832) = ldftn($Invoke2);
			*(data + 528860) = ldftn($Invoke3);
			*(data + 528888) = ldftn($Invoke4);
			*(data + 528916) = ldftn($Invoke5);
			*(data + 528944) = ldftn($Invoke6);
			*(data + 528972) = ldftn($Invoke7);
			*(data + 529000) = ldftn($Invoke8);
			*(data + 529028) = ldftn($Invoke9);
			*(data + 529056) = ldftn($Invoke10);
			*(data + 529084) = ldftn($Invoke11);
			*(data + 529112) = ldftn($Invoke12);
			*(data + 529140) = ldftn($Invoke13);
			*(data + 529168) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new BuildingTooltipController((UIntPtr)0);
		}
	}
}
