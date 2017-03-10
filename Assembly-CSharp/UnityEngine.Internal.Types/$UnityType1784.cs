using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1784 : $UnityType
	{
		public unsafe $UnityType1784()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 682916) = ldftn($Invoke0);
			*(data + 682944) = ldftn($Invoke1);
			*(data + 682972) = ldftn($Invoke2);
			*(data + 683000) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new SupportNode((UIntPtr)0);
		}
	}
}
