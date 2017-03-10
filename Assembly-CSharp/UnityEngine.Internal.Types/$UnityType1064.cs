using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1064 : $UnityType
	{
		public unsafe $UnityType1064()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 627196) = ldftn($Invoke0);
			*(data + 627224) = ldftn($Invoke1);
			*(data + 627252) = ldftn($Invoke2);
			*(data + 627280) = ldftn($Invoke3);
			*(data + 627308) = ldftn($Invoke4);
			*(data + 627336) = ldftn($Invoke5);
			*(data + 627364) = ldftn($Invoke6);
			*(data + 627392) = ldftn($Invoke7);
			*(data + 627420) = ldftn($Invoke8);
			*(data + 627448) = ldftn($Invoke9);
			*(data + 627476) = ldftn($Invoke10);
			*(data + 627504) = ldftn($Invoke11);
			*(data + 627532) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new SpatialIndex((UIntPtr)0);
		}
	}
}
