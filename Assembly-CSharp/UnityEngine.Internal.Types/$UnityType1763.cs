using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1763 : $UnityType
	{
		public unsafe $UnityType1763()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 679080) = ldftn($Invoke0);
			*(data + 679108) = ldftn($Invoke1);
			*(data + 679136) = ldftn($Invoke2);
			*(data + 679164) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new DroidHutNode((UIntPtr)0);
		}
	}
}
