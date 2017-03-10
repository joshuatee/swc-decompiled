using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1789 : $UnityType
	{
		public unsafe $UnityType1789()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 683756) = ldftn($Invoke0);
			*(data + 683784) = ldftn($Invoke1);
			*(data + 683812) = ldftn($Invoke2);
			*(data + 683840) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TransportNode((UIntPtr)0);
		}
	}
}
