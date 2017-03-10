using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1783 : $UnityType
	{
		public unsafe $UnityType1783()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 682804) = ldftn($Invoke0);
			*(data + 682832) = ldftn($Invoke1);
			*(data + 682860) = ldftn($Invoke2);
			*(data + 682888) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new StorageNode((UIntPtr)0);
		}
	}
}
