using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1766 : $UnityType
	{
		public unsafe $UnityType1766()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 679724) = ldftn($Invoke0);
			*(data + 679752) = ldftn($Invoke1);
			*(data + 679780) = ldftn($Invoke2);
			*(data + 679808) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new FactoryNode((UIntPtr)0);
		}
	}
}
