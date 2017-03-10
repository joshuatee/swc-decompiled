using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1764 : $UnityType
	{
		public unsafe $UnityType1764()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 679192) = ldftn($Invoke0);
			*(data + 679220) = ldftn($Invoke1);
			*(data + 679248) = ldftn($Invoke2);
			*(data + 679276) = ldftn($Invoke3);
			*(data + 679304) = ldftn($Invoke4);
			*(data + 679332) = ldftn($Invoke5);
			*(data + 679360) = ldftn($Invoke6);
			*(data + 679388) = ldftn($Invoke7);
			*(data + 679416) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new DroidNode((UIntPtr)0);
		}
	}
}
