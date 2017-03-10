using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1758 : $UnityType
	{
		public unsafe $UnityType1758()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 677736) = ldftn($Invoke0);
			*(data + 677764) = ldftn($Invoke1);
			*(data + 677792) = ldftn($Invoke2);
			*(data + 677820) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ChampionPlatformNode((UIntPtr)0);
		}
	}
}
