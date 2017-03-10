using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1695 : $UnityType
	{
		public unsafe $UnityType1695()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 667880) = ldftn($Invoke0);
			*(data + 667908) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ChampionPlatformComponent((UIntPtr)0);
		}
	}
}
