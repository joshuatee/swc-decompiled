using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType621 : $UnityType
	{
		public unsafe $UnityType621()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 508028) = ldftn($Invoke0);
			*(data + 508056) = ldftn($Invoke1);
			*(data + 508084) = ldftn($Invoke2);
			*(data + 508112) = ldftn($Invoke3);
			*(data + 508140) = ldftn($Invoke4);
			*(data + 508168) = ldftn($Invoke5);
			*(data + 508196) = ldftn($Invoke6);
			*(data + 508224) = ldftn($Invoke7);
			*(data + 508252) = ldftn($Invoke8);
			*(data + 508280) = ldftn($Invoke9);
			*(data + 508308) = ldftn($Invoke10);
			*(data + 508336) = ldftn($Invoke11);
			*(data + 508364) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new BuildingHoloEffect((UIntPtr)0);
		}
	}
}
