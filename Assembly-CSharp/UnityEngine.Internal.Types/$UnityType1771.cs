using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1771 : $UnityType
	{
		public unsafe $UnityType1771()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 680396) = ldftn($Invoke0);
			*(data + 680424) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new HealthViewNode((UIntPtr)0);
		}
	}
}
