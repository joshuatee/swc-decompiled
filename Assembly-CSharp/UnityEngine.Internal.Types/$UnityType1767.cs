using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1767 : $UnityType
	{
		public unsafe $UnityType1767()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 679836) = ldftn($Invoke0);
			*(data + 679864) = ldftn($Invoke1);
			*(data + 679892) = ldftn($Invoke2);
			*(data + 679920) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new FleetCommandNode((UIntPtr)0);
		}
	}
}
