using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1795 : $UnityType
	{
		public unsafe $UnityType1795()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 685156) = ldftn($Invoke0);
			*(data + 685184) = ldftn($Invoke1);
			*(data + 685212) = ldftn($Invoke2);
			*(data + 685240) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new WallNode((UIntPtr)0);
		}
	}
}
