using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1793 : $UnityType
	{
		public unsafe $UnityType1793()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 684708) = ldftn($Invoke0);
			*(data + 684736) = ldftn($Invoke1);
			*(data + 684764) = ldftn($Invoke2);
			*(data + 684792) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TurretBuildingNode((UIntPtr)0);
		}
	}
}
