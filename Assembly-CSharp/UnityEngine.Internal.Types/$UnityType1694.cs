using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1694 : $UnityType
	{
		public unsafe $UnityType1694()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 667824) = ldftn($Invoke0);
			*(data + 667852) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ChampionComponent((UIntPtr)0);
		}
	}
}
