using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1760 : $UnityType
	{
		public unsafe $UnityType1760()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 677960) = ldftn($Invoke0);
			*(data + 677988) = ldftn($Invoke1);
			*(data + 678016) = ldftn($Invoke2);
			*(data + 678044) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new DefenseLabNode((UIntPtr)0);
		}
	}
}
