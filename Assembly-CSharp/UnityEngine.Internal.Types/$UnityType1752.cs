using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1752 : $UnityType
	{
		public unsafe $UnityType1752()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 676952) = ldftn($Invoke0);
			*(data + 676980) = ldftn($Invoke1);
			*(data + 677008) = ldftn($Invoke2);
			*(data + 677036) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new BarracksNode((UIntPtr)0);
		}
	}
}
