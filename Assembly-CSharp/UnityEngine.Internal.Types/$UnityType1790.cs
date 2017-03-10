using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1790 : $UnityType
	{
		public unsafe $UnityType1790()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 683868) = ldftn($Invoke0);
			*(data + 683896) = ldftn($Invoke1);
			*(data + 683924) = ldftn($Invoke2);
			*(data + 683952) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TrapBuildingNode((UIntPtr)0);
		}
	}
}
