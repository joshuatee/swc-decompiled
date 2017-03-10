using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1775 : $UnityType
	{
		public unsafe $UnityType1775()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 681012) = ldftn($Invoke0);
			*(data + 681040) = ldftn($Invoke1);
			*(data + 681068) = ldftn($Invoke2);
			*(data + 681096) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new NavigationCenterNode((UIntPtr)0);
		}
	}
}
