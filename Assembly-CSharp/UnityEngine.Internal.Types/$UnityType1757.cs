using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1757 : $UnityType
	{
		public unsafe $UnityType1757()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 677680) = ldftn($Invoke0);
			*(data + 677708) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ChampionNode((UIntPtr)0);
		}
	}
}
