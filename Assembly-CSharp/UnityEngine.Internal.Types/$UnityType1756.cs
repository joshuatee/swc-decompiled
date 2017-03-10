using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1756 : $UnityType
	{
		public unsafe $UnityType1756()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 677568) = ldftn($Invoke0);
			*(data + 677596) = ldftn($Invoke1);
			*(data + 677624) = ldftn($Invoke2);
			*(data + 677652) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new CantinaNode((UIntPtr)0);
		}
	}
}
