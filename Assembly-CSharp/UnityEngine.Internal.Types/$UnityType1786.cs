using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1786 : $UnityType
	{
		public unsafe $UnityType1786()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 683252) = ldftn($Invoke0);
			*(data + 683280) = ldftn($Invoke1);
			*(data + 683308) = ldftn($Invoke2);
			*(data + 683336) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TacticalCommandNode((UIntPtr)0);
		}
	}
}
