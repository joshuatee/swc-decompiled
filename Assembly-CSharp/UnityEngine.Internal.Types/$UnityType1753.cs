using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1753 : $UnityType
	{
		public unsafe $UnityType1753()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 677064) = ldftn($Invoke0);
			*(data + 677092) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new BuffNode((UIntPtr)0);
		}
	}
}
